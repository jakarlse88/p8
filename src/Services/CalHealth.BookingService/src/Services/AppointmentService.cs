using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Messaging;
using CalHealth.BookingService.Messaging.Interfaces;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Serilog.ILogger;

namespace CalHealth.BookingService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentPublisher _appointmentPublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly Calendar _calendar;
        private readonly CultureInfo _cultureInfo;
        
        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentPublisher appointmentPublisher, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _appointmentPublisher = appointmentPublisher;
            _mapper = mapper;
            _cultureInfo = new CultureInfo("");
            _calendar = _cultureInfo.Calendar;
        }

        /// <summary>
        /// Create a new <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Appointment> CreateAsync(AppointmentDTO model)
        {
            if (model?.Patient == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = await GenerateEntity(model);

            await CheckDuplicate(entity);

            try
            {
                await _unitOfWork.AppointmentRepository.InsertAsync(entity);
                await _unitOfWork.CommitAsync();

                var message = GenerateMessage(model, entity);

                _appointmentPublisher.PushMessageToQueue(message);

                return entity;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Update an <see cref="Appointment"/> entity with a PatientId.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <param name="patientId"></param>
        /// <returns></returns>
        public async Task<Appointment> UpdatePatientIdAsync(int appointmentId, int patientId)
        {
            if (appointmentId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(appointmentId));
            }

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new Exception(
                    $"Received an unexpected null attempting to retrieve a {typeof(Appointment)} entity by the Id <{appointmentId}>");
            }

            try
            {
                appointment.PatientId = patientId;
                _unitOfWork.AppointmentRepository.Update(appointment);
                await _unitOfWork.CommitAsync();

                return appointment;
            }
            catch (Exception e)
            {
                Log.Error("An error has occurred: {@error}", e);
                throw;
            }
        }

        /// <summary>
        ///  Asynchronously get a single <see cref="Appointment"/> entity by its ID.
        /// </summary>
        /// <param name="appointmentId"></param>
        /// <returns></returns>
        public async Task<AppointmentDTO> GetByIdAsync(int appointmentId)
        {
            if (appointmentId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(appointmentId));
            }

            var result = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);

            if (result == null)
            {
                return null;
            }

            var mappedResult = _mapper.Map<AppointmentDTO>(result);

            return mappedResult;
        }

        /// <summary>
        /// Asynchronously get all <see cref="Appointment"/> entities by their associated <seealso cref="Consultant"/>.
        /// </summary>
        /// <param name="consultantId">The ID of the <seealso cref="Consultant"/> entity by which to query.</param>
        /// <returns></returns>
        public async Task<IEnumerable<AppointmentDTO>> GetByConsultantAsync(int consultantId)
        {
            if (consultantId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(consultantId));
            }

            var results = await _unitOfWork
                .AppointmentRepository
                .GetByConditionAsync(a => a.ConsultantId == consultantId);

            var mappedResults = _mapper.Map<IEnumerable<AppointmentDTO>>(results);

            return mappedResults;
        }

        /// <summary>
        /// Verify that there doesn't exist a duplicate <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task CheckDuplicate(Appointment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            
            var results = await _unitOfWork.AppointmentRepository.GetByConditionAsync(a =>
                a.ConsultantId == entity.Consultant.Id
                && a.WeekId == entity.Week.Id
                && a.TimeSlotId == entity.TimeSlot.Id
                && a.DayId == entity.Day.Id);
            
            // Log.Error("Results: {@results}", results);

            if (results.Any())
            // if (results.Any(a => a.Date.ToString("MM/dd/yyyy").Equals(entity.Date.ToString("MM/dd/yyyy"))))
            {
                throw new Exception("There already exists an appointment for this consultant at the specified time.");
            }
        }

        /// <summary>
        /// Generate a <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<Appointment> GenerateEntity(AppointmentDTO model)
        {
            var entity = new Appointment
            {
                Consultant = await GetConsultant(model),
                Date = model.Date,
                Week = await GetWeek(model),
                Day = await GetDayEntity(model),
                TimeSlot = await GetTimeSlot(model)
            };

            return entity;
        }

        private async Task<TimeSlot> GetTimeSlot(AppointmentDTO model)
        {
            return await _unitOfWork.TimeSlotRepository.GetByIdAsync(model.TimeSlotId);
        }

        private async Task<Week> GetWeek(AppointmentDTO model)
        {
            return await _unitOfWork.WeekRepository.GetByIdAsync(_calendar.GetWeekOfYear(model.Date,
                _cultureInfo.DateTimeFormat.CalendarWeekRule,
                _cultureInfo.DateTimeFormat.FirstDayOfWeek));
        }

        private async Task<Consultant> GetConsultant(AppointmentDTO model)
        {
            return await _unitOfWork.ConsultantRepository.GetByIdAsync(model.ConsultantId);
        }

        private async Task<Day> GetDayEntity(AppointmentDTO model)
        {
            var days = await _unitOfWork.DayRepository.GetByConditionAsync(d => d.Name == model.Date.ToString("dddd"));
            var day = days.FirstOrDefault();
            return day;
        }

        /// <summary>
        /// Generate a <see cref="AppointmentMessage"/>.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        private static AppointmentMessage GenerateMessage(AppointmentDTO model, Appointment entity)
        {
            var message = new AppointmentMessage
            {
                AppointmentId = entity.Id,
                ConsultantId = entity.ConsultantId,
                TimeSlotId = entity.TimeSlotId,
                Date = entity.Date,
                FirstName = model.Patient.FirstName,
                LastName = model.Patient.LastName,
                DateOfBirth = model.Patient.DateOfBirth
            };
            return message;
        }

        public void Dispose()
        {
        }
    }
}