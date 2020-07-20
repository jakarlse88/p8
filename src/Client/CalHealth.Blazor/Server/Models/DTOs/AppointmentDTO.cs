using System;

namespace CalHealth.Blazor.Server.Models
{
    public class AppointmentDTO
    {
        public int AppointmentId { get; set; }
        public int ConsultantId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
    }
}