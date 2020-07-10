using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Models;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.BookingService.Repositories
{
    public class TimeSlotRepository : RepositoryBase<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(BookingContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TimeSlot>> GetAllAsync()
        {
            var result =
                await base
                    .Get(_ => true)
                    .ToArrayAsync();

            return result;
        }
    }
}