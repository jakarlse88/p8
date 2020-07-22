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
using Serilog;

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
            _cultureInfo = new CultureInfo("en-US");
            _calendar = _cultureInfo.Calendar;
        }

        /// <summary>
        /// Create a new <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<AppointmentDTO> CreateAsync(AppointmentDTO model)
        {
            if (model?.Patient == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = await GenerateEntity(model);

            await CheckDuplicate(entity);

            try
            {
                _unitOfWork.AppointmentRepository.Add(entity);
                await _unitOfWork.CommitAsync();

                var message = GenerateMessage(model, entity);

                _appointmentPublisher.PushMessageToQueue(message);

                var mappedEntity = _mapper.Map<AppointmentDTO>(entity);

                return mappedEntity;
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
                await _unitOfWork.RollbackAsync();
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
                a.ConsultantId == entity.Consultant.Id);

            if (results.Any(a => a.WeekId == entity.Week.Id
                                 && a.TimeSlotId == entity.TimeSlot.Id
                                 && a.DayId == entity.Day.Id))
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var consultant = await GetConsultantEntityAsync(model);
            var week = await GetWeekEntityAsync(model);
            var day = await GetDayEntityAsync(model);
            var timeslot = await GetTimeSlotEntityAsync(model);
            
            var entity = new Appointment
            {
                Consultant = consultant,
                Date = model.Date,
                Week = week,
                Day = day,
                TimeSlot = timeslot 
            };
 
            return entity;
        }

        private async Task<TimeSlot> GetTimeSlotEntityAsync(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var result = await _unitOfWork.TimeSlotRepository.GetByIdAsync(model.TimeSlotId);
            
            return result;
        }

        private async Task<Week> GetWeekEntityAsync(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var result =  await _unitOfWork.WeekRepository.GetByIdAsync(_calendar.GetWeekOfYear(model.Date,
                _cultureInfo.DateTimeFormat.CalendarWeekRule,
                _cultureInfo.DateTimeFormat.FirstDayOfWeek));

            return result;
        }

        private async Task<Consultant> GetConsultantEntityAsync(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var result = await _unitOfWork.ConsultantRepository.GetByIdAsync(model.ConsultantId);
            
            return result;
        }

        private async Task<Day> GetDayEntityAsync(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            var days = await _unitOfWork.DayRepository.GetByConditionAsync(d =>
                d.Name == model.Date.ToString("dddd", CultureInfo.CreateSpecificCulture("en-US")));
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            
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