using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using CalHealth.Blazor.Client.Infrastructure.Exceptions;
using CalHealth.Blazor.Client.Infrastructure.Utilities;
using CalHealth.Blazor.Client.Services.Interfaces;

namespace CalHealth.Blazor.Client.Services
{
    public class ApiRequestService : IApiRequestService
    {
        private readonly HttpClient _httpClient;

        public ApiRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TEntity> HandleGetRequest<TEntity>(string requestUrl)
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }
            
            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUrl))
            using (var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken()))
            {
                var stream = await response.Content.ReadAsStreamAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = StreamUtilities.DeserializeJsonFromStream<TEntity>(stream);

                    return result;
                }

                var content = await StreamUtilities.StreamToStringAsync(stream);
                        
                throw new ApiException
                {
                    StatusCode = (int) response.StatusCode,
                    Content = content
                };
            }
        }

        public async Task<TEntity> HandlePostRequest<TEntity>(string requestUrl, TEntity dto)
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
            {
                throw new ArgumentNullException(nameof(requestUrl));
            }

            if (dto == null)
            {
                throw new ArgumentNullException(nameof(dto));
            }
            
            using (var request = new HttpRequestMessage(HttpMethod.Post, requestUrl))
            using (var httpContent = CreateHttpContent(dto))
            {
                request.Content = httpContent;

                using var response =
                    await _httpClient
                        .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, new CancellationToken())
                        .ConfigureAwait(false);

                var stream = await response.Content.ReadAsStreamAsync();
                
                if (response.IsSuccessStatusCode)
                {
                    var result = StreamUtilities.DeserializeJsonFromStream<TEntity>(stream);

                    return result;
                }
                
                var content = await StreamUtilities.StreamToStringAsync(stream);
                        
                throw new ApiException
                {
                    StatusCode = (int) response.StatusCode,
                    Content = content
                };
            }
        }
        
        // Original author: John Thiriet, https://johnthiriet.com
        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                StreamUtilities.SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return httpContent;
        }
    }
}