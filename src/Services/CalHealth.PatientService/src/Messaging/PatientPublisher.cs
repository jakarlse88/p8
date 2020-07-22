using System;
using System.Text;
using CalHealth.PatientService.Infrastructure.OptionsObjects;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Messaging.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class PatientPublisher : IPatientPublisher
    {
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }

        public PatientPublisher(IOptions<RabbitMqOptions> options)
        {
            Factory = new ConnectionFactory { HostName = options.Value.HostName };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }

        public bool PushMessageToQueue(PatientMessage entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var body =
                Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));

            Channel.BasicPublish(exchange: "patient",
                routingKey: "",
                basicProperties: null,
                body: body);

            Log.Information("Published successfully.");

            return true;
        }

        public void Register()
        {
            Channel.ExchangeDeclare(exchange: "patient", type: ExchangeType.Fanout);
        }

        public void Deregister()
        {
            Connection.Close();
        }
    }
}