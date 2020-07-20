using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CalHealth.Blazor.Client.Models
{
    public class ConsultantViewModel
    {
        public ConsultantViewModel()
        {
            Appointment = new List<AppointmentViewModel>();    
        }
        
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        
        public string Specialty { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public IList<AppointmentViewModel> Appointment { get; set; }
    }
}