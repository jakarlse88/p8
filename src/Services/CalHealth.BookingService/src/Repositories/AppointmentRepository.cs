using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(BookingContext context) : base(context)
        {
        }
    }
}