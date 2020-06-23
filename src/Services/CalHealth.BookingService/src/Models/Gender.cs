using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Gender
    {
        public Gender()
        {
            Patient = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
