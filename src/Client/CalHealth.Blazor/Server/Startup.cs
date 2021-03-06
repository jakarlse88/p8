using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using CalHealth.Blazor.Server.Hubs;
using CalHealth.Blazor.Server.Infrastructure.Extensions;
using CalHealth.Blazor.Server.Messaging;
using EasyNetQ;

namespace CalHealth.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var rabbitString = $"host={Configuration["RabbitMQ:HostName"]};";
            rabbitString += "virtualHost=" + (Configuration["RabbitMQ:VirtualHost"] ?? "/") + ";";
            rabbitString += $"username={Configuration["RabbitMQ:User"]};";
            rabbitString += $"password={Configuration["RabbitMQ:Password"]}";
            
            services.AddRazorPages();
            services.AddSignalR();
            services.AddControllersWithViews();
            services.AddOptionsObjects(Configuration);
            services.AddSingleton<IBus>(RabbitHutch.CreateBus(rabbitString));
            services.AddHostedService<AppointmentSubscriber>();
            services.AddResponseCompression(opts =>
            {
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "application/octet-stream" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseResponseCompression();
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<AppointmentHub>("/appointment");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
