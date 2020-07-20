using System;

namespace CalHealth.Blazor.Client.Models
{
    public class AppointmentMessage
    {
        public int AppointmentId { get; set; }
        public int ConsultantId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
    }
}