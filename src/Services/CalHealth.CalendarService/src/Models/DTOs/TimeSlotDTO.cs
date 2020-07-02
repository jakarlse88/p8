using System;

namespace CalHealth.CalendarService.Models.DTOs
{
    public class TimeSlotDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}