using System;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingDTO
    {
        public int TimeSlotId { get; set; }
        public string ConsultantId { get; set; }
        public DateTime Date { get; set; }
    }
}