using System.Collections.Generic;

namespace CalHealth.PatientService.Models
{
    public class Allergy : IEntityBase
    {
        public Allergy()
        {
            PatientAllergy = new HashSet<PatientAllergy>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<PatientAllergy> PatientAllergy { get; set; }
    }
}
