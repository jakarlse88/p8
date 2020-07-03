using System;
using System.Collections.Generic;

namespace CalHealth.Blazor.Client.Models
{
    public class ConsultantViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        
        public string Specialty { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}