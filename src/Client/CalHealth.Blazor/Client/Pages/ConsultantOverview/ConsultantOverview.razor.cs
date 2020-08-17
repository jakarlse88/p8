using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.Blazor.Client.Models;
using CalHealth.Blazor.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace CalHealth.Blazor.Client.Pages.ConsultantOverview
{
    public partial class ConsultantOverview
    {
        [Inject] private IApiRequestService ApiRequestService { get; set; }
        [Inject] private IWebAssemblyHostEnvironment Env { get; set; }
        private IEnumerable<ConsultantViewModel> Consultants { get; set; }
        private APIOperationStatus Status { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Status = APIOperationStatus.Initial;
            Consultants = new List<ConsultantViewModel>();
            var requestUrl =
                Env.IsDevelopment()
                    ? "https://localhost:5009/client-gw/consultant"
                    : "https://localhost:8088/client-gw/consultant";

            try
            {
                Consultants =
                    await ApiRequestService.HandleGetRequest<IEnumerable<ConsultantViewModel>>(requestUrl);
                Status = APIOperationStatus.GET_Success;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Status = APIOperationStatus.GET_Error;
            }

            StateHasChanged();
            await base.OnInitializedAsync();
        }
    }
}