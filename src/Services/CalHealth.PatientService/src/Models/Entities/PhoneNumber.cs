using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public class PhoneNumber : IEntityBase
    {
        public PhoneNumber()
        {
            PatientPhoneNumber = new HashSet<PatientPhoneNumber>();
        }

        public int Id { get; set; }
        public string Number { get; set; }

        public ICollection<PatientPhoneNumber> PatientPhoneNumber { get;  }
    }
}
