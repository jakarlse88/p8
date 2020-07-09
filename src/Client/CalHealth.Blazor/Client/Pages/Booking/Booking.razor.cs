using System;
using System.Collections.Generic;
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
        private BookingDTO InputModel { get; set; }
        private BookingViewModel ViewModel { get; set; }
        private EditContext ScheduleEditContext { get; set; }
        private EditContext PatientEditContext { get; set; }
        private bool ConsultantIsValid { get; set; }
        private bool DateIsValid { get; set; }
        private bool PatientIsValid { get; set; }
        private APIOperationStatus Status { get; set; }
        private bool FormIsValid => ConsultantIsValid 
                                    && DateIsValid 
                                    && PatientIsValid;

        /// <summary>
        /// Component initialization logic.
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            Status = APIOperationStatus.GET_Pending;

            ViewModel = new BookingViewModel();
            InputModel = new BookingDTO();

            ConsultantIsValid = false;
            PatientIsValid = false;

            PatientEditContext = new EditContext(InputModel.Patient);
            PatientEditContext.OnFieldChanged += HandlePatientEditContextFieldChanged;

            ScheduleEditContext = new EditContext(InputModel.Schedule);
            ScheduleEditContext.OnFieldChanged += HandleDateEditContextFieldChanged;
            ScheduleEditContext.OnFieldChanged += HandleConsultantFieldChanged;

            try
            {
                ViewModel = await FetchBookingInfo();
                ViewModel.Allergies = new List<AllergyViewModel>
                {
                    new AllergyViewModel
                    {
                        Id = 1,
                        Type = "Gluten"
                    },
                    new AllergyViewModel
                    {
                        Id = 2,
                        Type = "Lactose"
                    },
                    new AllergyViewModel
                    {
                        Id = 3,
                        Type = "Latex"
                    },
                    new AllergyViewModel
                    {
                        Id = 1,
                        Type = "Gluten"
                    },
                    new AllergyViewModel
                    {
                        Id = 2,
                        Type = "Lactose"
                    },
                    new AllergyViewModel
                    {
                        Id = 3,
                        Type = "Latex"
                    }
                };
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

        private async Task HandleSubmit()
        {
            const string requestUrl = "https://localhost:8088/gateway/appointment";
            Status = APIOperationStatus.POST_Pending;
            StateHasChanged();

            // TODO: Validation?

            try
            {
                var result = await ApiRequestService.HandlePostRequest(requestUrl, InputModel);

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

        private async Task<BookingViewModel> FetchBookingInfo()
        {
            const string requestUrl = "https://localhost:8088/gateway/booking-aggregate";

            var result =
                await ApiRequestService.HandleGetRequest<BookingViewModel>(requestUrl);

            return result;
        }

        private void HandleDateEditContextFieldChanged(object sender, EventArgs ea)
        {
            Console.WriteLine("asd");
            // TODO: We will likely have to perform some validation on the selected date against consultant availability here at some point.
            DateIsValid = true;
            
            StateHasChanged();
        }

        private void HandleConsultantFieldChanged(object sender, EventArgs ea)
        {
            ConsultantIsValid = !string.IsNullOrWhiteSpace(InputModel.Schedule.ConsultantIdProxy)
                                && InputModel.Schedule.ConsultantIdProxy != "0";
            
            StateHasChanged();
        }

        private void HandlePatientEditContextFieldChanged(object sender, EventArgs ea)
        {
            PatientIsValid = PatientEditContext.Validate();
            StateHasChanged();
        }

        public void Dispose()
        {
            ScheduleEditContext.OnFieldChanged -= HandleDateEditContextFieldChanged;
            PatientEditContext.OnFieldChanged -= HandlePatientEditContextFieldChanged;
            ScheduleEditContext.OnFieldChanged -= HandleConsultantFieldChanged;
        }
    }
}