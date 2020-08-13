namespace CalHealth.BookingService.Messaging.Interfaces
{
    public interface IAppointmentPublisher
    {
        bool PushMessageToQueue(AppointmentMessage entity);
    }
}