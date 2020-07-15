namespace CalHealth.Blazor.Server.Messaging
{
    public interface IAppointmentSubscriber
    {
        void Register();
        void Deregister();
    }
}