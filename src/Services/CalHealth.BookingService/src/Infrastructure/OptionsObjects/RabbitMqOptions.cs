namespace CalHealth.BookingService.Infrastructure
{
    public class RabbitMqOptions
    {
        public const string RabbitMq = "RabbitMQ";
        
        public string HostName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}