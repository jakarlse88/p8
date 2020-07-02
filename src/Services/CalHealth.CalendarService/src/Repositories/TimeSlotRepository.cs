using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Models;
using CalHealth.CalendarService.Repositories.Interfaces;

namespace CalHealth.CalendarService.Repositories
{
    public class TimeSlotRepository : RepositoryBase<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(CalendarContext context) : base(context)
        {
        }
    }
}