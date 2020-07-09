using System;
using System.Collections.Generic;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            Consultants = new List<ConsultantViewModel>();
            TimeSlots = new List<TimeSlotViewModel>();
            Genders = new List<GenderViewModel>();
            Allergies = new List<AllergyViewModel>();
            Religions = new List<ReligionViewModel>();
        }

        public IEnumerable<ConsultantViewModel> Consultants { get; }
        public IEnumerable<TimeSlotViewModel> TimeSlots { get; }
        public IEnumerable<GenderViewModel> Genders { get; set; }
        public IEnumerable<AllergyViewModel> Allergies { get; set; }
        public IEnumerable<ReligionViewModel> Religions { get; set; }
    }
}