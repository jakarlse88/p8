using System.Threading.Tasks;

namespace CalHealth.BookingService.Repositories
{
    public interface IUnitOfWork
    {
        ITimeSlotRepository TimeSlotRepository { get; }
        IConsultantRepository ConsultantRepository { get; }
        IAppointmentRepository AppointmentRepository { get; }
        
        Task CommitAsync();
        Task RollbackAsync();
    }
}