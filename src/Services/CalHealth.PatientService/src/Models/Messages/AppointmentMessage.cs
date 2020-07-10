using System;

namespace CalHealth.PatientService.Models
{
    public class AppointmentMessage
    {
        public int AppointmentId { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}