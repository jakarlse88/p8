using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public class TimeSlot
    {
        public TimeSlot()
        {
            Appointment = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ICollection<Appointment> Appointment { get; set; }
    }
}
