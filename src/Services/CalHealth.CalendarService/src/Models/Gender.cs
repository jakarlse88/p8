using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Consultant = new HashSet<Consultant>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Consultant> Consultant { get; set; }
    }
}
