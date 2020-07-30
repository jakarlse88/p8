using System;
using System.Text;
using CalHealth.PatientService.Infrastructure.OptionsObjects;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Messaging.Messages;
using CalHealth.PatientService.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CalHealth.PatientService.Messaging
{
    public class AppointmentSubscriber : IAppointmentSubscriber
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }

        public AppointmentSubscriber(IServiceScopeFactory scopeFactory, IOptions<RabbitMqOptions> options)
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
            Channel.ExchangeDeclare(exchange: "appointment", type: ExchangeType.Fanout);

            var queueName = Channel.QueueDeclare().QueueName;
            Channel.QueueBind(queue: queueName,
                exchange: "appointment",
                routingKey: "");

            var consumer = new EventingBasicConsumer(Channel);

            consumer.Received += ConsumerOnReceived();

            Channel.BasicQos(0, 100, false);
            Channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        private EventHandler<BasicDeliverEventArgs> ConsumerOnReceived()
        {
            return async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<AppointmentMessage>(Encoding.UTF8.GetString(body));

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    using (var service = scope.ServiceProvider.GetRequiredService<IPatientService>())
                    {
                        await service.HandleIncomingPatientData(message);
                    }

                    Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    Channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };
        }

        public void Deregister()
        {
            Connection.Close();
        }
    }
}