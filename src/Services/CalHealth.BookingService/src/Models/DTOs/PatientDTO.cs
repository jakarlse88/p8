using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public class PatientDTO
    {
        public int GenderId { get; set; }
        
        public int? ReligionId { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public IEnumerable<int> AllergyList { get; set; }
    }
}