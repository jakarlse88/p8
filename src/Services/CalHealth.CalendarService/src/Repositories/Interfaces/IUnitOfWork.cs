using System.Threading.Tasks;

namespace CalHealth.CalendarService.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        ITimeSlotRepository TimeSlotRepository { get; }
        IConsultantRepository ConsultantRepository { get; }
        
        Task CommitAsync();
        Task RollbackAsync();
    }
}