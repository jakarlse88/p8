namespace CalHealth.Blazor.Client.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public int TimeSlotId { get; set; }
        public int? NoteId { get; set; }
    }
}