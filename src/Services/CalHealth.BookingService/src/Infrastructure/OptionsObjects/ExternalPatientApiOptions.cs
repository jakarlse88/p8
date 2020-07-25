namespace CalHealth.BookingService.Infrastructure
{
    public class ExternalPatientApiOptions
    {
        public const string ExternalPatientApi = "ExternalPatientApi";
        public string Protocol { get; set; }
        public string HostName { get; set; }
        public string Port { get; set; }
    }
}