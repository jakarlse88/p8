using System;

namespace CalHealth.BookingService.Messaging
{
    public class AppointmentMessage
    {
        public int AppointmentId { get; set; }
        public int ConsultantId { get; set; }
        public int TimeSlotId { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}