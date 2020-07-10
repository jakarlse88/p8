using System.Collections.Generic;

namespace CalHealth.PatientService.Models
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
