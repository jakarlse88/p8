namespace CalHealth.PatientService.Services
{
    public interface IAppointmentSubscriber
    {
        void Register();
        void Deregister();
    }
}