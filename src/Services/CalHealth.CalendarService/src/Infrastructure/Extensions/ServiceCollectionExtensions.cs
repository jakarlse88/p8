using System;
using System.IO;
using System.Reflection;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Repositories;
using CalHealth.CalendarService.Repositories.Interfaces;
using CalHealth.CalendarService.Services;
using CalHealth.CalendarService.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace CalHealth.CalendarService.Infrastructure.Extensions
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
            services.AddTransient<ITimeSlotService, TimeSlotService>();

            return services;
        }
        
        internal static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
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
                });
            });
        }
        
        internal static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CalendarContext>(opt =>
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
                    Description = "Californian Health Calendar Service API",
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