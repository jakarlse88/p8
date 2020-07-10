using System;
using System.Text;
using System.Threading;
using CalHealth.PatientService.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Serilog;

namespace CalHealth.PatientService.Services
{
    public class PatientPublisher : IPatientPublisher
    {
        private ConnectionFactory Factory { get; }
        private IConnection Connection { get; set; }
        private IModel Channel { get; }

        public PatientPublisher()
        {
            Thread.Sleep(60000); // TODO This is a temporary hack to allow RabbitMQ to come up when using docker-compose
            Factory = new ConnectionFactory { HostName = "rabbitmq" };
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