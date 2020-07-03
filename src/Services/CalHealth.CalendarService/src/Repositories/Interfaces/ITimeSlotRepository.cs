using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.CalendarService.Models;

namespace CalHealth.CalendarService.Repositories.Interfaces
{
    public interface ITimeSlotRepository
    {
        Task<IEnumerable<TimeSlot>> GetAllAsync();
    }
}