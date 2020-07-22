using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IAppointmentService : IDisposable
    {
        Task<AppointmentDTO> CreateAsync(AppointmentDTO model);
        Task<Appointment> UpdatePatientIdAsync(int appointmentId, int patientId);
        Task<AppointmentDTO> GetByIdAsync(int appointmentId);
        Task<IEnumerable<AppointmentDTO>> GetByConsultantAsync(int consultantId);
    }
}