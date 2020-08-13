using System;
using System.Text;
using CalHealth.BookingService.Infrastructure;
using CalHealth.BookingService.Messaging.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace CalHealth.BookingService.Messaging
{
    public class AppointmentPublisher : IAppointmentPublisher
    {
        private readonly IConnection _connection;
        
        public AppointmentPublisher(IOptions<RabbitMqOptions> options)
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
                Log.Error("AppointmentPublisher initialisation error: {@error}", e);
            }
        }

        public bool PushMessageToQueue(AppointmentMessage entity)
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
                    channel.ExchangeDeclare(exchange: "appointment", type: ExchangeType.Fanout);
                
                    channel.BasicPublish(exchange: "appointment",
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
    }
}