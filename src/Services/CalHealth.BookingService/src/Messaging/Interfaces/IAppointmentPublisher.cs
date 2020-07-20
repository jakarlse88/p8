namespace CalHealth.BookingService.Messaging.Interfaces
{
    public interface IAppointmentPublisher
    {
        bool PushMessageToQueue(AppointmentMessage entity);
        public void Register();
        public void Deregister();
    }
}