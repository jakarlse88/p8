using System;

namespace CalHealth.BookingService.Models
{
    public class ConsultantDTO
    {
        public int Id { get; set; }
        public string Specialty { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}