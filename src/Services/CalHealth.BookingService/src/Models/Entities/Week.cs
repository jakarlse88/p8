using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Week
    {
        public Week()
        {
            Appointment = new HashSet<Appointment>();
            ConsultantAvailabilityPerWeek = new HashSet<ConsultantAvailabilityPerWeek>();
        }

        public int Id { get; set; }
        public byte Number { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<ConsultantAvailabilityPerWeek> ConsultantAvailabilityPerWeek { get; set; }
    }
}
