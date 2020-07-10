using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public partial class Religion
    {
        public Religion()
        {
            Patient = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
