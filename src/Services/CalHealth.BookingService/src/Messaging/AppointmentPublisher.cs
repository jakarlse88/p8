using System;
using CalHealth.BookingService.Messaging.Interfaces;
using CalHealth.Messages;
using EasyNetQ;

namespace CalHealth.BookingService.Messaging
{
    public class AppointmentPublisher : IAppointmentPublisher
    {
        private readonly IBus _bus;
        
        public AppointmentPublisher(IBus bus)
        {
            _bus = bus;
        }

        public bool PushMessageToQueue(AppointmentMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                _bus.Publish<AppointmentMessage>(message);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while attempting to emit an event: {@ex}", e);
                return false;
            }
            
            return true;
        }
    }
}