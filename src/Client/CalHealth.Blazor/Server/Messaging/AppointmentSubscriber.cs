using System;
using System.Text;
using CalHealth.Blazor.Server.Hubs;
using CalHealth.Blazor.Server.Infrastructure.OptionsObjects;
using CalHealth.Blazor.Server.Messaging.Messages;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using CalHealth.Blazor.Server.Models;
using Microsoft.Extensions.Options;

namespace CalHealth.Blazor.Server.Messaging
{
    public class AppointmentSubscriber : IAppointmentSubscriber
    {
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; }
        private IModel Channel { get; }
        private readonly IHubContext<AppointmentHub> _hubContext;

        public AppointmentSubscriber(IHubContext<AppointmentHub> hubContext, IOptions<RabbitMqOptions> options)
        {
            _hubContext = hubContext;
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
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = JsonConvert.DeserializeObject<AppointmentMessage>(Encoding.UTF8.GetString(body));

                try
                {
                    var dto = new AppointmentDTO
                    {
                        AppointmentId = message.AppointmentId,
                        ConsultantId = message.ConsultantId,
                        TimeSlotId = message.TimeSlotId,
                        Date = message.Date
                    };

                    // ReSharper disable once MethodHasAsyncOverload
                    var payload = JsonConvert.SerializeObject(dto);
                    
                    await _hubContext.Clients.All.SendAsync("Appointment", dto);

                    Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                    Channel.BasicNack(ea.DeliveryTag, false, false);
                    throw;
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