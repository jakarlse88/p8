using System;
using System.IO;
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

namespace CalHealth.BookingService.Infrastructure.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddRepositoryLayer(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        internal static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services
                .AddTransient<IAppointmentService, AppointmentService>()
                .AddTransient<ITimeSlotService, TimeSlotService>()
                .AddTransient<IConsultantService, ConsultantService>();

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
        
        internal static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
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
    }
}