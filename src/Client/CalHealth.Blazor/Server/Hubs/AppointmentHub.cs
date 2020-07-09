using System;
using System.Threading.Tasks;
using CalHealth.Blazor.Server.Models;
using Microsoft.AspNetCore.SignalR;

namespace CalHealth.Blazor.Server.Hubs
{
    public class AppointmentHub : Hub
    {
        public async Task SendMessage(AppointmentDTO model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            
            await Clients.All.SendAsync("ReceiveAppointment", model);
        }
    }
}