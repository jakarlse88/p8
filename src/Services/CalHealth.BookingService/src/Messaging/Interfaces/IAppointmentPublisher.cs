using CalHealth.Messages;

namespace CalHealth.BookingService.Messaging.Interfaces
{
    public interface IAppointmentPublisher
    {
        bool PushMessageToQueue(AppointmentMessage message);
    }
}