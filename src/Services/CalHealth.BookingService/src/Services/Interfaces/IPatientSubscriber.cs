namespace CalHealth.BookingService.Services
{
    public interface IPatientSubscriber
    {
        public void Register();
        public void Deregister();
    }
}