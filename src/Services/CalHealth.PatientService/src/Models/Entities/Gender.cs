using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public class Gender : IEntityBase
    {
        public Gender()
        {
            Patient = new HashSet<Patient>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<Patient> Patient { get; }
    }
}
