using System;

namespace CalHealth.Blazor.Client.Models
{
    public class AppointmentDTO
    {
        public AppointmentDTO()
        {
            Patient = new PatientDTO();
        }
        
        public int ConsultantId { get; set; }
        public DateTime Date { get; set; }
        public int TimeSlotId { get; set; }

        public PatientDTO Patient { get; set; }
    }
}