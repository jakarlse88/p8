using System;
using System.Threading.Tasks;
using CalHealth.BookingService.Services;
using CalHealth.Messages;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace CalHealth.BookingService.Messaging
{
    public class PatientSubscriber : RabbitSubscriber
    {
        private readonly IServiceProvider _services;
        
        public PatientSubscriber(IBus bus, IServiceProvider services) : base(bus)
        {
            _services = services;
        }

        protected override async Task<bool> Process(PatientMessage message)
        {
            if (message == null)
            {
                return false;
            }
            
            try
            {
                using (var scope = _services.CreateScope())
                using (var service = scope.ServiceProvider.GetRequiredService<IAppointmentService>())
                {
                    await service.UpdatePatientIdAsync(message.AppointmentId, message.PatientId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occurred: {@error}", e);
            }
            
            return true;
        }
    }
}