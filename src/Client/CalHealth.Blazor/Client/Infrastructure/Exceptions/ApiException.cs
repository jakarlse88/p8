using System;

namespace CalHealth.Blazor.Client.Infrastructure.Exceptions
{
    public class ApiException : Exception
    {
        public int StatusCode { get; set; }
        public string Content { get; set; }
    }
}