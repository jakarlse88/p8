using System;
using System.Net.Http;
using System.Threading.Tasks;
using CalHealth.Blazor.Client.Services;
using CalHealth.Blazor.Client.Services.Interfaces;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace CalHealth.Blazor.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddTransient<IApiRequestService, ApiRequestService>();

            await builder.Build().RunAsync();
        }
    }
}
