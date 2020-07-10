namespace CalHealth.BookingService.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public int TimeSlotId { get; set; }
        public int? NoteId { get; set; }
        public int PatientId { get; set; }

        public virtual Consultant Consultant { get; set; }
        public virtual Day Day { get; set; }
        public virtual Note Note { get; set; }
        public virtual TimeSlot TimeSlot { get; set; }
        public virtual Week Week { get; set; }
    }
}
