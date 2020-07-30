using System;
using System.Text;
using CalHealth.BookingService.Infrastructure;
using CalHealth.BookingService.Messaging.Interfaces;
using CalHealth.BookingService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CalHealth.BookingService.Messaging
{
    public class PatientSubscriber : IPatientSubscriber
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }

        public PatientSubscriber(IServiceScopeFactory scopeFactory, IOptions<RabbitMqOptions> options)
        {
            _scopeFactory = scopeFactory;
            Factory = new ConnectionFactory
            {
                HostName = options.Value.HostName,
                UserName = options.Value.User,
                Password = options.Value.Password
            };
            Connection = Factory.CreateConnection();
            Channel = Connection.CreateModel();
        }
        
        public void Register()
        {
            Channel.ExchangeDeclare(exchange: "patient", type: ExchangeType.Fanout);

            var queueName = Channel.QueueDeclare().QueueName;
            Channel.QueueBind(queue: queueName,
                exchange: "patient",
                routingKey: "");
            
            var consumer = new EventingBasicConsumer(Channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<PatientMessage>(Encoding.UTF8.GetString(body));

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    using (var service = scope.ServiceProvider.GetRequiredService<IAppointmentService>())
                    {
                        await service.UpdatePatientIdAsync(message.AppointmentId, message.PatientId);
                    }

                    Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception e)
                {
                    Log.Error("An error has occurred: {@error}", e);
                    Channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            Channel.BasicQos(0, 100, false);
            Channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }
        
        public void Deregister()
        {
            Connection.Close();
        }
    }
}