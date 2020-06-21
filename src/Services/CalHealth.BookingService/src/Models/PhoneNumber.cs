using System;
using System.Collections.Generic;

namespace CalHealth.BookingService.Models
{
    public partial class PhoneNumber
    {
        public PhoneNumber()
        {
            PatientPhoneNumber = new HashSet<PatientPhoneNumber>();
        }

        public int Id { get; set; }
        public string Number { get; set; }

        public virtual ICollection<PatientPhoneNumber> PatientPhoneNumber { get; set; }
    }
}
