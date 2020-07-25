using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using CalHealth.BookingService.Infrastructure.Extensions;
using Polly;
using Polly.Extensions.Http;

namespace CalHealth.BookingService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services
                .AddMemoryCache()
                .AddAutoMapper(typeof(Startup))
                .ConfigureDbContext(Configuration)
                .AddOptionsObjects(Configuration)
                .AddRepositoryLayer()
                .AddServiceLayer(Configuration)
                .AddMessagingLayer()
                .ConfigureSwagger()
                .ConfigureCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.ApplyMigrations()
                .UseAppointmentPublisher()
                .UsePatientSubscriber()
                .UseCustomExceptionHandler()
                .UseCors()
                .UseRouting()
                .UseAuthorization()
                .UseSwaggerUI()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}