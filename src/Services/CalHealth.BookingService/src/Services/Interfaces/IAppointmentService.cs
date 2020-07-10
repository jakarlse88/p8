using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IAppointmentService
    {
        Task<Appointment> CreateAsync(AppointmentDTO model);
    }
}