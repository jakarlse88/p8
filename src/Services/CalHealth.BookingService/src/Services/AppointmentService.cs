using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
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
        private readonly Calendar _calendar;
        private readonly CultureInfo _cultureInfo;

        public AppointmentService(IUnitOfWork unitOfWork, IAppointmentPublisher appointmentPublisher)
        {
            _unitOfWork = unitOfWork;
            _appointmentPublisher = appointmentPublisher;
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

            var entity = GenerateEntity(model);

            await CheckDuplicate(model, entity);

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

        public async Task<Appointment> UpdatePatientIdAsync(int appointmentId, int patientId)
        {
            if (appointmentId < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(appointmentId));
            }

            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
            {
                throw new Exception($"Received an unexpected null attempting to retrieve a {typeof(Appointment)} entity by the Id <{appointmentId}>");
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
        /// Verify that there doesn't exist a duplicate <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task CheckDuplicate(AppointmentDTO model, Appointment entity)
        {
            var result = await _unitOfWork.AppointmentRepository.GetByConditionAsync(a =>
                a.ConsultantId == model.ConsultantId
                && a.WeekId == entity.WeekId
                && a.TimeSlotId == entity.TimeSlotId
                && a.DayId == entity.DayId);

            if (result.Any())
            {
                throw new Exception("There already exists an appointment for this consultant at the specified time.");
            }
        }

        /// <summary>
        /// Generate a <see cref="Appointment"/> entity.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private Appointment GenerateEntity(AppointmentDTO model)
        {
            var entity = new Appointment
            {
                ConsultantId = model.ConsultantId,
                Date = model.Date,
                WeekId = _calendar.GetWeekOfYear(model.Date, _cultureInfo.DateTimeFormat.CalendarWeekRule,
                    _cultureInfo.DateTimeFormat.FirstDayOfWeek),
                DayId = (int) _calendar.GetDayOfWeek(model.Date),
                TimeSlotId = model.TimeSlotId,
            };
            return entity;
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