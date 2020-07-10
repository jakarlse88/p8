using System;

namespace CalHealth.PatientService.Models
{
    public class PatientDTO
    {
        public int Id { get; set; }
        public int GenderId { get; set; }
        public int? ReligionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}