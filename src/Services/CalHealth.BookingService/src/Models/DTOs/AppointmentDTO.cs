using System;
using System.ComponentModel.DataAnnotations;

namespace CalHealth.BookingService.Models
{
    public class AppointmentDTO
    {
        public int Id { get; set; }
        
        [Range(1, int.MaxValue)]
        public int TimeSlotId { get; set; }
        
        [Range(1, int.MaxValue)]
        public int ConsultantId { get; set; }
        
        public DateTime Date { get; set; }

        [Required]
        public PatientDTO Patient { get; set; }
    }
}