using CalHealth.Messages;

namespace CalHealth.PatientService.Messaging.Interfaces
{
    public interface IPatientPublisher
    {
        bool PushMessageToQueue(PatientMessage message);
    }
}