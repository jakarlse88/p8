using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using CalHealth.BookingService.Services;
using CalHealth.BookingService.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Messaging;
using CalHealth.BookingService.Messaging.Interfaces;
using Polly;
using Polly.Extensions.Http;

namespace CalHealth.BookingService.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddOptionsObjects(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RabbitMqOptions>(configuration.GetSection(RabbitMqOptions.RabbitMq));
            services.Configure<ExternalPatientApiOptions>(
                configuration.GetSection(ExternalPatientApiOptions.ExternalPatientApi));

            return services;
        }

        internal static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        internal static IServiceCollection AddServiceLayer(this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddTransient<IAppointmentService, AppointmentService>()
                .AddTransient<ITimeSlotService, TimeSlotService>()
                .AddTransient<IConsultantService, ConsultantService>()
                .AddHttpClient<IExternalPatientApiService, ExternalPatientApiService>(cfg =>
                {
                    var options = new ExternalPatientApiOptions();
                    configuration.GetSection(ExternalPatientApiOptions.ExternalPatientApi).Bind(options);

                    cfg.BaseAddress = new Uri($"{options.Protocol}://{options.HostName}:{options.Port}");
                })
                .AddPolicyHandler(GetRetryPolicy());
                // .AddPolicyHandler(GetCircuitBreakerPolicy());

            return services;
        }

        internal static IServiceCollection AddMessagingLayer(this IServiceCollection services)
        {
            services
                .AddSingleton<IAppointmentPublisher, AppointmentPublisher>()
                .AddSingleton<IPatientSubscriber, PatientSubscriber>();

            return services;
        }

        internal static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddDefaultPolicy(builder =>
            {
                builder // For testing using docker-compose
                    .WithOrigins("https://localhost:8081")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                builder // For testing using docker-compose
                    .WithOrigins("http://localhost:8080")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                builder // For testing with non-container client
                    .WithOrigins("http://localhost:5000")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                builder // For testing with non-container client
                    .WithOrigins("https://localhost:5001")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            }));
        }

        internal static IServiceCollection ConfigureDbContext(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<BookingContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        internal static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Calendar Service",
                    Version = "v1",
                    Description = "Californian Health Booking Service API",
                    Contact = new OpenApiContact
                    {
                        Name = "Jon Karlsen",
                        Email = "karlsen.jonarild@gmail.com",
                        Url = new Uri("https://github.com/jakarlse88")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                config.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            var jitterer = new Random();
            
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))  
                                    + TimeSpan.FromMilliseconds(jitterer.Next(0, 100))
                );
        }
        
        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}