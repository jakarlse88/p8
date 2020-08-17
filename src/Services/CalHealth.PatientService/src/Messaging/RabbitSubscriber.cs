using System.Threading;
using System.Threading.Tasks;
using CalHealth.Messages;
using EasyNetQ;
using Microsoft.Extensions.Hosting;

namespace CalHealth.PatientService.Messaging
{
    public abstract class RabbitSubscriber : IHostedService
    {
        private readonly IBus _bus;

        public RabbitSubscriber(IBus bus)
        {
            _bus = bus;
        }

        protected abstract Task<bool> Process(AppointmentMessage message);

        public void Register()
        {
            _bus.SubscribeAsync<AppointmentMessage>("appointment", Process);
        }

        public void Deregister()
        {
            _bus.Dispose();
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