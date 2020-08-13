using System;
using System.Threading.Tasks;
using CalHealth.PatientService.Infrastructure.OptionsObjects;
using CalHealth.PatientService.Messaging.Messages;
using CalHealth.PatientService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class AppointmentSubscriber : RabbitSubscriber
    {
        private readonly IServiceProvider _services;
        
        public AppointmentSubscriber(IServiceProvider services, IOptions<RabbitMqOptions> options) 
            : base(options)
        {
            _services = services;
        }

        protected override async Task<bool> Process(string message)
        {
            if (message == null)
            {
                return false;
            }
            
            try
            {
                var deserialized = JsonConvert.DeserializeObject<AppointmentMessage>(message);
                    
                using (var scope = _services.CreateScope())
                using (var service = scope.ServiceProvider.GetRequiredService<IPatientService>())
                {
                    await service.HandleIncomingPatientData(deserialized);
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