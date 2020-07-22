using System.Threading.Tasks;

namespace CalHealth.Blazor.Client.Services.Interfaces
{
    public interface IApiRequestService
    {
        Task<TEntity> HandleGetRequest<TEntity>(string requestUrl);
        Task<TEntity> HandlePostRequest<TEntity>(string requestUrl, TEntity dto);
    }
}