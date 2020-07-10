using System;
using System.Collections.Generic;

namespace CalHealth.PatientService.Messaging.Messages
{
    public class AppointmentMessage
    {
        public int AppointmentId { get; set; }
        public int GenderId { get; set; }
        
        public int? ReligionId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<int> AllergyList { get; set; }
    }
}