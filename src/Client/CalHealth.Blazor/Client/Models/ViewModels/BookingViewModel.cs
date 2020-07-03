using System;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingViewModel
    {
        public int TimeSlotId { get; set; }
        public string ConsultantId { get; set; }
        public DateTime Date { get; set; }
    }
}