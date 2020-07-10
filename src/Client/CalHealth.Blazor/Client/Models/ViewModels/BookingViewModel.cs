using System.Collections.Generic;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            ConsultantList = new List<ConsultantViewModel>();
            TimeSlotList = new List<TimeSlotViewModel>();
            GenderList = new List<GenderViewModel>();
            AllergyList = new List<AllergyViewModel>();
            ReligionList = new List<ReligionViewModel>();
        }

        public IEnumerable<ConsultantViewModel> ConsultantList { get; }
        public IEnumerable<TimeSlotViewModel> TimeSlotList { get; }
        public IEnumerable<GenderViewModel> GenderList { get; }
        public IEnumerable<AllergyViewModel> AllergyList { get; }
        public IEnumerable<ReligionViewModel> ReligionList { get; }
    }
}