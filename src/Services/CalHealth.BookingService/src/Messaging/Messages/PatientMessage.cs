namespace CalHealth.BookingService.Messaging
{
    public class PatientMessage
    {
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
    }
}