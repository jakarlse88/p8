using System;

namespace CalHealth.BookingService.Models
{
    public class TimeSlotDTO
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}