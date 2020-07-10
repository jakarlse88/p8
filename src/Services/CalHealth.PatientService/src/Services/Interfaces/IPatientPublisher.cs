using CalHealth.PatientService.Models;

namespace CalHealth.PatientService.Services
{
    public interface IPatientPublisher
    {
        bool PushMessageToQueue(PatientMessage entity);
        void Register();
        void Deregister();
    }
}