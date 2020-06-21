using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class Week
    {
        public Week()
        {
            Appointment = new HashSet<Appointment>();
        }

        public int Id { get; set; }
        public byte Number { get; set; }

        public virtual ICollection<Appointment> Appointment { get; set; }
    }
}
