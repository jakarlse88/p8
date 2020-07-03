using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CalHealth.Blazor.Client.Models;
using CalHealth.Blazor.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace CalHealth.Blazor.Client.Pages.Booking
{
    public partial class Booking : IDisposable
    {
        [Inject] private IApiRequestService ApiRequestService { get; set; }
        [Inject] private NavigationManager NavigationManager { get; set; }
        private BookingViewModel Model { get; set; }
        private IEnumerable<ConsultantViewModel> Consultants { get; set; }
        private EditContext DateEditContext { get; set; }
        private bool DateIsValid { get; set; }
        private IEnumerable<TimeSlotViewModel> TimeSlots { get; set; }
        private APIOperationStatus Status { get; set; }

        /// <summary>
        /// Component initialization logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Model = new BookingViewModel { Date = DateTime.Today };
            Consultants = new List<ConsultantViewModel>();
            DateEditContext = new EditContext(Model.Date);
            DateEditContext.OnFieldChanged += HandleEditContextFieldChanged;

            try
            {
                Consultants = await FetchConsultants();
                TimeSlots = await FetchTimeSlots();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Status = APIOperationStatus.GET_Error;
            }

            StateHasChanged();
            await base.OnInitializedAsync();
        }

        private async Task HandleSubmit()
        {
            const string requestUrl = "https://localhost:8083/api/Booking";
            Status = APIOperationStatus.POST_Pending;
            StateHasChanged();

            var dto = new BookingDTO
            {
                ConsultantId = Model.ConsultantId,
                Date = Model.Date,
                TimeSlotId = Model.TimeSlotId
            };

            try
            {
                var result = await ApiRequestService.HandlePostRequest<BookingDTO>(requestUrl, dto);

                Status = APIOperationStatus.POST_Success;
                StateHasChanged();
                
                // TODO: /Appointment/Id page?
                // NavigationManager.NavigateTo(result.ConsultantId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task<IEnumerable<ConsultantViewModel>> FetchConsultants()
        {
            const string requestUrl = "https://localhost:8085/api/Consultant";
        
            var result =
                await ApiRequestService.HandleGetRequest<IEnumerable<ConsultantViewModel>>(requestUrl);

            return result;
        }

        private async Task<IEnumerable<TimeSlotViewModel>> FetchTimeSlots()
        {
            const string requestUrl = "https://localhost:8085/api/TimeSlot";
            
            var result = await ApiRequestService.HandleGetRequest<IEnumerable<TimeSlotViewModel>>(requestUrl);

            return result;
        }
        
        private void HandleEditContextFieldChanged(object sender, EventArgs args)
        {
            // We will likely have to perform some validation on the selected date against 
            // consultant availability here at some point.
            DateIsValid = true;
        }
        
        public void Dispose()
        {
            DateEditContext.OnFieldChanged -= HandleEditContextFieldChanged;
        }
    }
}