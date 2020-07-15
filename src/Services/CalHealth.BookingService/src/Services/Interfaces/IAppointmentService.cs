using System;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IAppointmentService : IDisposable
    {
        Task<Appointment> CreateAsync(AppointmentDTO model);
        Task<Appointment> UpdatePatientIdAsync(int appointmentId, int patientId);
    }
}