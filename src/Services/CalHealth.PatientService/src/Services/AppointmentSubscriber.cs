using System.Text;
using System.Threading;
using CalHealth.PatientService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;

namespace CalHealth.PatientService.Services
{
    public class AppointmentSubscriber : IAppointmentSubscriber
    {
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; set; }
        private IModel Channel { get; }

        public AppointmentSubscriber()
        {
            Thread.Sleep(60000); // TODO This is a temporary hack to allow RabbitMQ to come up when using docker-compose
            Factory = new ConnectionFactory { HostName = "rabbitmq" };
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
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var model = JsonConvert.DeserializeObject<AppointmentMessage>(Encoding.UTF8.GetString(body));

                // Handle model
                Log.Information("appointmentMessage received: {appointment}", model);

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