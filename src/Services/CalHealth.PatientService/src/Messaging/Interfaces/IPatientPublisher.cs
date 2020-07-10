using CalHealth.PatientService.Messaging.Messages;

namespace CalHealth.PatientService.Messaging.Interfaces
{
    public interface IPatientPublisher
    {
        bool PushMessageToQueue(PatientMessage entity);
        void Register();
        void Deregister();
    }
}