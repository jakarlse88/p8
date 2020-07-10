using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Note : IEntityBase
    {
        public Note()
        {
            Appointment = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public DateTime TimeCreated { get; set; }
        public DateTime TimeLastUpdated { get; set; }
        public string Content { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
