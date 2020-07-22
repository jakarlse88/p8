using System.Collections.Generic;
using Newtonsoft.Json;

namespace CalHealth.Blazor.Client.Models
{
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            ConsultantList = new List<ConsultantViewModel>();
            TimeSlotList = new List<TimeSlotViewModel>();
            PatientList = new List<PatientViewModel>();
        }

        [JsonProperty("Consultant")]
        public IEnumerable<ConsultantViewModel> ConsultantList { get; }
        
        [JsonProperty("TimeSlot")]
        public IEnumerable<TimeSlotViewModel> TimeSlotList { get; }

        [JsonProperty("Patient")]
        public IEnumerable<PatientViewModel> PatientList { get; set; }
        
    }
}