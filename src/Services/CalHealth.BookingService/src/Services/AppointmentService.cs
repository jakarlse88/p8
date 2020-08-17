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
using CalHealth.Messages;
using Microsoft.Extensions.Logging;

namespace CalHealth.BookingService.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentPublisher _appointmentPublisher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;
        private readonly Calendar _calendar;
        private readonly CultureInfo _cultureInfo;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, IAppointmentPublisher appointmentPublisher, ILogger<AppointmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _appointmentPublisher = appointmentPublisher;
            _logger = logger;
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
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (model.Patient == null)
            {
                throw new ArgumentNullException(nameof(model.Patient));
            }
            
            var entity = await GenerateEntity(model);

            if (model.Date.Date < DateTime.Today.Date
                || await CheckIdenticalEntry(entity))
            {
                return null;
            }
            
            try
            {
                await PersistToDb(entity);

                EmitMessage(model, entity);

                var mappedEntity = _mapper.Map<AppointmentDTO>(entity);
                
                return mappedEntity;
            }
            catch (Exception e)
            {
                _logger.LogError($"An exception was raised while attempting to create an appointment: {e}", e);
                await _unitOfWork.RollbackAsync();
                return null;
            }
        }

        private async Task PersistToDb(Appointment entity)
        {
            _unitOfWork.AppointmentRepository.Add(entity);
            await _unitOfWork.CommitAsync();
        }

        private void EmitMessage(AppointmentDTO model, Appointment entity)
        {
            var message = GenerateMessage(model, entity);

            _appointmentPublisher.PushMessageToQueue(message);
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
                _logger.LogError($"An error has occurred: {e}", e);
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
        /// Checks whether an identical <see cref="Appointment"/> entity already exists in the DB.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>A bool indicating whether or not an identical entry already exists.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<bool> CheckIdenticalEntry(Appointment entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var results = await _unitOfWork.AppointmentRepository.GetByConditionAsync(a =>
                a.ConsultantId == entity.Consultant.Id);

            var exists = results.Any(a => a.WeekId == entity.Week.Id
                                          && a.TimeSlotId == entity.TimeSlot.Id
                                          && a.DayId == entity.Day.Id);

            return exists;
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

            var result = await _unitOfWork.WeekRepository.GetByIdAsync(_calendar.GetWeekOfYear(model.Date,
                _cultureInfo.DateTimeFormat.CalendarWeekRule,
                _cultureInfo.DateTimeFormat.FirstDayOfWeek) % 52);

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