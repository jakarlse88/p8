using System;
using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public class Patient : IEntityBase
    {
        public Patient()
        {
            PatientAddress = new HashSet<PatientAddress>();
            PatientAllergy = new HashSet<PatientAllergy>();
            PatientPhoneNumber = new HashSet<PatientPhoneNumber>();
        }

        public int Id { get; set; }
        public int GenderId { get; set; }
        public int? ReligionId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        public Religion Religion { get; set; }
        public ICollection<PatientAddress> PatientAddress { get; }
        public ICollection<PatientAllergy> PatientAllergy { get; set; }
        public ICollection<PatientPhoneNumber> PatientPhoneNumber { get; }
    }
}
