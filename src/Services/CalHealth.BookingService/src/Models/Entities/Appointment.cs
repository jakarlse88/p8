using System;

namespace CalHealth.BookingService.Models
{
    public class Appointment : IEntityBase
    {
        public int Id { get; set; }
        public int ConsultantId { get; set; }
        public DateTime Date { get; set; }
        public int WeekId { get; set; }
        public int DayId { get; set; }
        public int TimeSlotId { get; set; }
        public int? NoteId { get; set; }
        public int PatientId { get; set; }

        public Consultant Consultant { get; set; }
        public Day Day { get; set; }
        public Note Note { get; set; }
        public TimeSlot TimeSlot { get; set; }
        public Week Week { get; set; }
    }
}
