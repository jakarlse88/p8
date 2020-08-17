using System;
using System.Threading.Tasks;
using CalHealth.Messages;
using CalHealth.PatientService.Services;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class AppointmentSubscriber : RabbitSubscriber
    {
        private readonly IServiceProvider _services;
        
        public AppointmentSubscriber(IServiceProvider services, IBus bus) 
            : base(bus)
        {
            _services = services;
        }

        protected override async Task<bool> Process(AppointmentMessage message)
        {
            if (message == null)
            {
                return false;
            }
            
            try
            {
                using (var scope = _services.CreateScope())
                using (var service = scope.ServiceProvider.GetRequiredService<IPatientService>())
                {
                    await service.HandleIncomingPatientData(message);
                }
            }
            catch (Exception e)
            {
                Log.Error("An error has occurred: {@error}", e);
            }
            
            return true;
        }
    }
}