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
        private BookingFormViewModel FormModel { get; set; }
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
            FormModel = new BookingFormViewModel();

            ConsultantIsValid = false;
            PatientIsValid = false;

            PatientEditContext = new EditContext(FormModel.Patient);
            PatientEditContext.OnFieldChanged += HandlePatientEditContextFieldChanged;

            ScheduleEditContext = new EditContext(FormModel.Schedule);
            ScheduleEditContext.OnFieldChanged += HandleDateEditContextFieldChanged;
            ScheduleEditContext.OnFieldChanged += HandleConsultantFieldChanged;

            try
            {
                ViewModel = await FetchBookingInfo();
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

        /// <summary>
        /// Handle form submission.
        /// </summary>
        /// <returns></returns>
        private async Task HandleSubmit()
        {
            const string requestUrl = "https://localhost:8088/client-gw/appointment";
            Status = APIOperationStatus.POST_Pending;
            StateHasChanged();

            try
            {
                var dto = CreateDTO();
                
                var result = await ApiRequestService.HandlePostRequest(requestUrl, dto);

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

        /// <summary>
        /// Create the data transfer object for form submission.
        /// </summary>
        /// <returns></returns>
        private AppointmentDTO CreateDTO()
        {
            var dto = new AppointmentDTO
            {
                ConsultantId = FormModel.Schedule.ConsultantId,
                Date = FormModel.Schedule.Date,
                TimeSlotId = FormModel.Schedule.TimeSlotId,
                Patient = new PatientDTO
                {
                    GenderId = FormModel.Patient.GenderId,
                    ReligionId = FormModel.Patient.ReligionId == 0 ? null : (int?) FormModel.Patient.ReligionId,
                    FirstName = FormModel.Patient.FirstName,
                    LastName = FormModel.Patient.LastName,
                    DateOfBirth = FormModel.Patient.DateOfBirth,
                    AllergyList = GenerateAllergyList()
                }
            };
            
            return dto;
        }

        /// <summary>
        /// Generate a list containing the id of each selected allergy.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<int> GenerateAllergyList()
        {
            var selectedAllergies = new List<int>();

            foreach (var model in ViewModel.AllergyList)
            {
                if (model.Selected)
                {
                    selectedAllergies.Add(model.Id);
                }
            }

            return selectedAllergies;
        }

        /// <summary>
        /// Fetch information/options from the API services.
        /// </summary>
        /// <returns></returns>
        private async Task<BookingViewModel> FetchBookingInfo()
        {
            const string requestUrl = "https://localhost:8088/client-gw/aggregate";

            var result =
                await ApiRequestService.HandleGetRequest<BookingViewModel>(requestUrl);

            return result;
        }

        /// <summary>
        /// Handle date field change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void HandleDateEditContextFieldChanged(object sender, EventArgs ea)
        {
            Console.WriteLine("asd");
            // TODO: We will likely have to perform some validation on the selected date against consultant availability here at some point.
            DateIsValid = true;
            
            StateHasChanged();
        }

        /// <summary>
        /// Handle consultant field change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void HandleConsultantFieldChanged(object sender, EventArgs ea)
        {
            ConsultantIsValid = !string.IsNullOrWhiteSpace(FormModel.Schedule.ConsultantIdProxy)
                                && FormModel.Schedule.ConsultantIdProxy != "0";
            
            StateHasChanged();
        }

        /// <summary>
        /// Handle patient fields change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void HandlePatientEditContextFieldChanged(object sender, EventArgs ea)
        {
            PatientIsValid = PatientEditContext.Validate();
            StateHasChanged();
        }

        /// <summary>
        /// Tidy up. :)
        /// </summary>
        public void Dispose()
        {
            ScheduleEditContext.OnFieldChanged -= HandleDateEditContextFieldChanged;
            PatientEditContext.OnFieldChanged -= HandlePatientEditContextFieldChanged;
            ScheduleEditContext.OnFieldChanged -= HandleConsultantFieldChanged;
        }
    }
}