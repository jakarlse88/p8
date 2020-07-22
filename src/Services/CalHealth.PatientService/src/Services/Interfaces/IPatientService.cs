using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CalHealth.PatientService.Messaging.Messages;
using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Services
{
    public interface IPatientService : IDisposable
    {
        Task HandleIncomingPatientData(AppointmentMessage message);
        Task<IEnumerable<PatientDTO>> GetAllAsync();
    }
}