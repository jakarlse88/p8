using System;

namespace CalHealth.Blazor.Client.Models
{
    public class PatientViewModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
    }
}