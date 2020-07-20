namespace CalHealth.BookingService.Messaging.Interfaces
{
    public interface IPatientSubscriber
    {
        public void Register();
        public void Deregister();
    }
}