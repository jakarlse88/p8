using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CalHealth.CalendarService.Repositories
{
    public class TimeSlotRepository : RepositoryBase<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(CalendarContext context) : base(context)
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