using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CalHealth.BookingService.Infrastructure;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CalHealth.BookingService.Messaging
{
    public abstract class RabbitSubscriber : IHostedService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitSubscriber(IOptions<RabbitMqOptions> options)
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
                _channel = _connection.CreateModel();
            }
            catch (Exception e)
            {
                Log.Error("RabbitSubscriber initialisation error: {@error}", e);
            }
        }

        protected abstract Task<bool> Process(string message);

        public void Register()
        {
            _channel.ExchangeDeclare(exchange: "patient", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName,
                exchange: "patient",
                routingKey: "");
            
            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var result = await Process(message);

                if (result)
                {
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);        
                }
                else
                {
                    _channel.BasicNack(ea.DeliveryTag, false, false);
                }

            };

            _channel.BasicQos(0, 10, false);
            _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);
        }

        public void Deregister()
        {
            _connection.Close();
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Register();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Deregister();
            return Task.CompletedTask;
        }
    }
}