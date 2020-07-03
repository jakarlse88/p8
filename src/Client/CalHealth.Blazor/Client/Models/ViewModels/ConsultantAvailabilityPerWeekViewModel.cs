namespace CalHealth.Blazor.Client.Models
{
    public class ConsultantAvailabilityPerWeekViewModel
    {
        public int ConsultantId { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public bool Available { get; set; }
    }
}