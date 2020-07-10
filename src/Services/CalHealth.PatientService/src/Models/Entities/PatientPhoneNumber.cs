namespace CalHealth.PatientService.Models
{
    public partial class PatientPhoneNumber
    {
        public int PatientId { get; set; }
        public int PhoneNumberId { get; set; }

        public virtual Patient Patient { get; set; }
        public virtual PhoneNumber PhoneNumber { get; set; }
    }
}
