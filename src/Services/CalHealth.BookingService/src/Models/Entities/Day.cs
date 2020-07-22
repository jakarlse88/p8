using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Day : IEntityBase
    {
        public Day()
        {
            Appointment = new HashSet<Appointment>();
            ConsultantAvailabilityPerWeek = new HashSet<ConsultantAvailabilityPerWeek>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
        public virtual ICollection<ConsultantAvailabilityPerWeek> ConsultantAvailabilityPerWeek { get; set; }
    }
}
