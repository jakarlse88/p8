using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class Consultant
    {
        public Consultant()
        {
            Appointment = new HashSet<Appointment>();
            ConsultantAvailabilityPerWeek = new HashSet<ConsultantAvailabilityPerWeek>();
        }

        public int Id { get; set; }
        public int SpecialtyId { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public virtual Gender Gender { get; set; }
        public virtual Specialty Specialty { get; set; }
        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<ConsultantAvailabilityPerWeek> ConsultantAvailabilityPerWeek { get; set; }
    }
}
