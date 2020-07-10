using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Repositories
{
    public interface ITimeSlotRepository
    {
        Task<IEnumerable<TimeSlot>> GetAllAsync();
    }
}