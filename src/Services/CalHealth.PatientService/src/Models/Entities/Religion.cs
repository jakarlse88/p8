using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public class Religion : IEntityBase
    {
        public Religion()
        {
            Patient = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Patient> Patient { get; }
    }
}
