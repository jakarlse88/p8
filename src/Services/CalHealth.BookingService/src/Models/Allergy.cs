using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class Allergy
    {
        public Allergy()
        {
            PatientAllergy = new HashSet<PatientAllergy>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual ICollection<PatientAllergy> PatientAllergy { get; set; }
    }
}
