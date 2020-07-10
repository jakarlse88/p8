using CalHealth.BookingService.Models;

namespace CalHealth.BookingService.Services
{
    public interface IAppointmentPublisher
    {
        bool PushMessageToQueue(AppointmentMessage entity);
        public void Register();
        public void Deregister();
    }
}