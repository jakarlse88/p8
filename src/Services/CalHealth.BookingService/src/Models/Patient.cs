using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Note = new HashSet<Note>();
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

        public virtual Gender Gender { get; set; }
        public virtual Religion Religion { get; set; }
        public virtual ICollection<Note> Note { get; set; }
        public virtual ICollection<PatientAddress> PatientAddress { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergy { get; set; }
        public virtual ICollection<PatientPhoneNumber> PatientPhoneNumber { get; set; }
    }
}
