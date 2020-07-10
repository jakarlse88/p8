﻿using System.Text;
using System.Threading;
using CalHealth.BookingService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CalHealth.BookingService.Services
{
    public class PatientSubscriber : IPatientSubscriber
    {
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; set; }
        private IModel Channel { get; }

        public PatientSubscriber()
        {
            Thread.Sleep(60000); // TODO This is a temporary hack to allow RabbitMQ to come up when using docker-compose
            Factory = new ConnectionFactory { HostName = "rabbitmq" };
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
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var model = JsonConvert.DeserializeObject<PatientMessage>(Encoding.UTF8.GetString(body));
                
                // Handle model
                Log.Information("PatientMessage received: {patient}", model);
                
                Channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            Channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
        
        public void Deregister()
        {
            Connection.Close();
        }
    }
}