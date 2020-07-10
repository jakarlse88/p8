using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using CalHealth.BookingService.Models;
using CalHealth.BookingService.Repositories;

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
            _cultureInfo = new CultureInfo("EN-us");
            _calendar = _cultureInfo.Calendar;
        }

        public async Task<Appointment> CreateAsync(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var entity = new Appointment
            {
                ConsultantId = model.ConsultantId,
                WeekId = _calendar.GetWeekOfYear(model.Date, _cultureInfo.DateTimeFormat.CalendarWeekRule, _cultureInfo.DateTimeFormat.FirstDayOfWeek),
                DayId = (int) model.Date.DayOfWeek,
                TimeSlotId = model.TimeSlotId,
                PatientId = model.PatientId
            };

            try
            {
                _unitOfWork.AppointmentRepository.Create(entity);

                // TODO: Before offloading to PatientService, query said service to check whether entity exists
                // TODO: public IActionResult GetPatientId(PatientDTO model) -- { bool Exists, int PatientId }

                _appointmentPublisher.PushMessageToQueue(new AppointmentMessage { AppointmentId = entity.PatientId });
                await _unitOfWork.CommitAsync();
                return entity;
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}