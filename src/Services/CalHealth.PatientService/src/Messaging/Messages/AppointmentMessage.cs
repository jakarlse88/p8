using System;

namespace CalHealth.PatientService.Messaging.Messages
{
    public class AppointmentMessage
    {
        public int AppointmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}