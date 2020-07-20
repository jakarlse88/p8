using System;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingFormViewModel
    {
        public BookingFormViewModel()
        {
            Schedule = new ScheduleFormViewModel();
            Patient = new PatientFormViewModel();
        }

        public ScheduleFormViewModel Schedule { get; }
        public PatientFormViewModel Patient { get; }
    }
}