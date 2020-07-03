using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.Blazor.Client.Models;
using CalHealth.Blazor.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace CalHealth.Blazor.Client.Pages.ConsultantOverview
{
    public partial class ConsultantOverview
    {
        [Inject] private IApiRequestService ApiRequestService { get; set; }
        private const string RequestUrl = "https://localhost:8085/api/Consultant";
        private IEnumerable<ConsultantViewModel> Consultants { get; set; }
        private APIOperationStatus Status { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Status = APIOperationStatus.Initial;
            Consultants = new List<ConsultantViewModel>();

            try
            {
                Consultants =
                    await ApiRequestService.HandleGetRequest<IEnumerable<ConsultantViewModel>>(RequestUrl);
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