// ReSharper disable InconsistentNaming
namespace CalHealth.Blazor.Client.Models
{
    public enum APIOperationStatus
    {
        Initial,
        GET_Pending,
        GET_Success,
        GET_Error,
        POST_Pending,
        POST_Success,
        POST_Error,
    }
}