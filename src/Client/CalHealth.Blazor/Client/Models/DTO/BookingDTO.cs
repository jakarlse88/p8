using System;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingDTO
    {
        public BookingDTO()
        {
            Schedule = new ScheduleDTO();
            Patient = new PatientDTO();
        }

        public ScheduleDTO Schedule { get; }
        public PatientDTO Patient { get; }
    }
}