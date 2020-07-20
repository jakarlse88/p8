using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public class Consultant : IEntityBase 
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

        public Gender Gender { get; set; }
        public Specialty Specialty { get; set; }
        public ICollection<Appointment> Appointment { get; set; }
        public ICollection<ConsultantAvailabilityPerWeek> ConsultantAvailabilityPerWeek { get; set; }
    }
}
