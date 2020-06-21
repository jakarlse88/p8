using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Note
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string Content { get; set; }

        public virtual Patient Patient { get; set; }
    }
}
