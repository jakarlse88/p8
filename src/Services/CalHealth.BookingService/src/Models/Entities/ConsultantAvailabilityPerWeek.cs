namespace CalHealth.BookingService.Models
{
    public partial class ConsultantAvailabilityPerWeek
    {
        public int ConsultantId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public bool Available { get; set; }

        public virtual Consultant Consultant { get; set; }
        public virtual Day Day { get; set; }
        public virtual Week Week { get; set; }
    }
}
