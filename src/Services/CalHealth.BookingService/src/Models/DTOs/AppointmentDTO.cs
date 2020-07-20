using System;

namespace CalHealth.BookingService.Models
{
    public class AppointmentDTO
    {
        public int TimeSlotId { get; set; }
        public int ConsultantId { get; set; }
        public DateTime Date { get; set; }

        public PatientDTO Patient { get; set; }
    }
}