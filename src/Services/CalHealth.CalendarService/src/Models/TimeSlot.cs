using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class TimeSlot
    {
        public TimeSlot()
        {
            Appointment = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
