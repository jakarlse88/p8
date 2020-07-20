using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public class ConsultantDTO
    {
        public ConsultantDTO()
        {
            Appointment = new HashSet<AppointmentDTO>();
        }
        public int Id { get; set; }
        public string Specialty { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }

        public ICollection<AppointmentDTO> Appointment { get; set; }
    }
}