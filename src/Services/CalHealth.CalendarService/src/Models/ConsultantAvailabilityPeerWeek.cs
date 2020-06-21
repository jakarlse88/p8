using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class ConsultantAvailabilityPeerWeek
    {
        public int ConsultantId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public bool Available { get; set; }

        public virtual Consultant Consultant { get; set; }
    }
}
