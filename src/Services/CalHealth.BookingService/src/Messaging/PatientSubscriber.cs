using System;
using System.Threading.Tasks;
using CalHealth.BookingService.Infrastructure;
using CalHealth.BookingService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;

namespace CalHealth.BookingService.Messaging
{
    public class PatientSubscriber : RabbitSubscriber
    {
        private readonly IServiceProvider _services;
        
        public PatientSubscriber(IOptions<RabbitMqOptions> options, IServiceProvider services) : base(options)
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
                var deserialized = JsonConvert.DeserializeObject<PatientMessage>(message);
                    
                using (var scope = _services.CreateScope())
                using (var service = scope.ServiceProvider.GetRequiredService<IAppointmentService>())
                {
                    await service.UpdatePatientIdAsync(deserialized.AppointmentId, deserialized.PatientId);
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