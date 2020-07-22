using System;
using System.Collections.Generic;

namespace CalHealth.Blazor.Client.Models
{
    public class PatientDTO
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
    }
}