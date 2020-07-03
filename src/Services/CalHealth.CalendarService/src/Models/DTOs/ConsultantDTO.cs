using System;

namespace CalHealth.CalendarService.Models.DTOs
{
    public class ConsultantDTO
    {
        public int Id { get; set; }
        public int SpecialtyId { get; set; }
        public int GenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}