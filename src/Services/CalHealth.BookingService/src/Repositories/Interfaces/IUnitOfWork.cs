using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<TimeSlot> TimeSlotRepository { get; }
        IRepository<Consultant> ConsultantRepository { get; }
        IRepository<Appointment> AppointmentRepository { get; }
        IRepository<Day> DayRepository { get; }
        IRepository<Week> WeekRepository { get; }

        Task CommitAsync();
        Task RollbackAsync();
    }
}