using System;
using System.Threading.Tasks;
using CalHealth.Blazor.Server.Hubs;
using CalHealth.Messages;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using EasyNetQ;
using AppointmentDTO = CalHealth.Blazor.Server.Models.AppointmentDTO;

namespace CalHealth.Blazor.Server.Messaging
{
    public class AppointmentSubscriber : RabbitSubscriber
    {
        private readonly IHubContext<AppointmentHub> _hubContext;

        public AppointmentSubscriber(IBus bus, IHubContext<AppointmentHub> hubContext)
            : base(bus)
        {
            _hubContext = hubContext;
        }

        protected override async Task<bool> Process(AppointmentMessage message)
        {
            if (message == null)
            {
                return false;
            }

            try
            {
                var dto = new AppointmentDTO
                {
                    AppointmentId = message.AppointmentId,
                    ConsultantId = message.ConsultantId,
                    TimeSlotId = message.TimeSlotId,
                    Date = message.Date
                };

                await _hubContext.Clients.All.SendAsync("Appointment", dto);
            }
            catch (Exception e)
            {
                Log.Error("An error has occurred: {@error}", e);
            }

            return true;
        }
    }
}