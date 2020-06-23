﻿using System;
using System.Collections.Generic;

namespace CalHealth.CalendarService.Models
{
    public partial class Specialty
    {
        public Specialty()
        {
            Consultant = new HashSet<Consultant>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Consultant> Consultant { get; set; }
    }
}
