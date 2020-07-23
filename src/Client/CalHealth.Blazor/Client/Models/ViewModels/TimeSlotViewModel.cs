using System;

namespace CalHealth.Blazor.Client.Models
{
    public class TimeSlotViewModel
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool Available { get; set; } = true;
    }
}