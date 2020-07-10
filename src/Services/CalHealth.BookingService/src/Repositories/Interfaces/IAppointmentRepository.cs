using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public interface IAppointmentRepository
    {
        Appointment Create(Appointment entity);
    }
}