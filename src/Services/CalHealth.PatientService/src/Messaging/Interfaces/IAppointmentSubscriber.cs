namespace CalHealth.PatientService.Messaging.Interfaces
{
    public interface IAppointmentSubscriber
    {
        void Register();
        void Deregister();
    }
}