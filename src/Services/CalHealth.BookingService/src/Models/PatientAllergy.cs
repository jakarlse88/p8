using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class PatientAllergy
    {
        public int PatientId { get; set; }
        public int AllergyId { get; set; }

        public virtual Allergy Allergy { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
