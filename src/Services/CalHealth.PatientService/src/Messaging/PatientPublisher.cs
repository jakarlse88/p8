using System;
using System.Text;
using CalHealth.PatientService.Infrastructure.OptionsObjects;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Messaging.Messages;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class PatientPublisher : IPatientPublisher, IDisposable
    {
        private readonly IConnection _connection;

        public PatientPublisher(IOptions<RabbitMqOptions> options)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = options.Value.HostName,
                    UserName = options.Value.User,
                    Password = options.Value.Password,
                    DispatchConsumersAsync = true
                };

                _connection = factory.CreateConnection();
            }
            catch (Exception e)
            {
                Log.Error("PatientPublisher initialisation error: {@error}", e);
            }
        }

        public bool PushMessageToQueue(PatientMessage entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            try
            {
                var body =
                    Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));

                using (var channel = _connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "patient", type: ExchangeType.Fanout);
                
                    channel.BasicPublish(exchange: "patient",
                        routingKey: "",
                        basicProperties: null,
                        body: body);
                }
            }
            catch (Exception e)
            {
                Log.Error("An error occurred while attempting to emit an event: {@ex}", e);
                return false;
            }
            
            return true;
        }

        public void Dispose()
        {
            _connection.Close();
        }
    }
}