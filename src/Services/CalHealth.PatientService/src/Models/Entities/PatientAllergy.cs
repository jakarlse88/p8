namespace CalHealth.PatientService.Models
{
    public class PatientAllergy
    {
        public int PatientId { get; set; }
        public int AllergyId { get; set; }

        public virtual Allergy Allergy { get; set; }
        public virtual Patient Patient { get; set; }
    }
}
