using System;
using System.Text;
using CalHealth.Messages;
using CalHealth.PatientService.Infrastructure.OptionsObjects;
using CalHealth.PatientService.Messaging.Interfaces;
using EasyNetQ;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class PatientPublisher : IPatientPublisher
    {
        private readonly IBus _bus;

        public PatientPublisher(IOptions<RabbitMqOptions> options, IBus bus)
        {
            _bus = bus;
        }

        public bool PushMessageToQueue(PatientMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            
            try
            {
                _bus.Publish<PatientMessage>(message);
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while attempting to emit an event: {@ex}", e);
                return false;
            }
            
            return true;
        }
    }
}