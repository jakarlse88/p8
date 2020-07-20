using System;
using System.Net;
using CalHealth.BookingService.Data;
using CalHealth.BookingService.Extensions;
using CalHealth.BookingService.Messaging.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;

namespace CalHealth.BookingService.Infrastructure.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        private static IAppointmentPublisher AppointmentPublisher { get; set; }
        private static IPatientSubscriber PatientSubscriber { get; set; }
        
        /// <summary>
        /// Set up the <see cref="AppointmentPublisher"/> messaging service.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UseAppointmentPublisher(this IApplicationBuilder app)
        {
            AppointmentPublisher = app.ApplicationServices.GetService<IAppointmentPublisher>();

            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnAppointmentPublisherStarted);
            lifetime.ApplicationStopping.Register(OnAppointmentPublisherStopping);

            return app;
        }
        
        /// <summary>
        /// Register the <see cref="AppointmentPublisher"/> messaging service on app startup.
        /// </summary>
        private static void OnAppointmentPublisherStarted()
        {
            AppointmentPublisher.Register();
        }

        /// <summary>
        /// Deregister the <see cref="AppointmentPublisher"/> messaging service on app shutdown.
        /// </summary>
        private static void OnAppointmentPublisherStopping()
        {
            AppointmentPublisher.Deregister();
        }

        /// <summary>
        /// Set up the <see cref="PatientSubscriber"/> messaging service.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        internal static IApplicationBuilder UsePatientSubscriber(this IApplicationBuilder app)
        {
            PatientSubscriber = app.ApplicationServices.GetService<IPatientSubscriber>();
            
            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            
            lifetime.ApplicationStarted.Register(OnPatientSubscriberStarted);
            lifetime.ApplicationStopping.Register(OnPatientSubscriberStopping);

            return app;
        }
        
        /// <summary>
        /// Register the <see cref="PatientSubscriber"/> messaging service on app startup.
        /// </summary>
        private static void OnPatientSubscriberStarted()
        {
            PatientSubscriber.Register();
        }

        /// <summary>
        /// Deregister the <see cref="PatientSubscriber"/> messaging service on app shutdown.
        /// </summary>
        private static void OnPatientSubscriberStopping()
        {
            PatientSubscriber.Deregister();
        }
        
        internal static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Californian Health Calendar Service API 1.0");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }
        internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            Log.Information("Applying migrations. This may take a moment.");

            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<BookingContext>())
            {
                try
                {
                    var retry =
                        Policy
                        .Handle<SqlException>()
                        .WaitAndRetry(new[]
                        {
                            TimeSpan.FromSeconds(3),
                            TimeSpan.FromSeconds(6),
                            TimeSpan.FromSeconds(9)
                        });

                    retry.Execute(() => context.Database.Migrate());
                }
                catch (Exception ex)
                {
                    Log.Fatal("A fatal error occurred while trying to apply migrations: {ex}", ex);
                    throw;
                }
            }

            Log.Information("Migrations operation successful. Continuing application Startup.");
            return app;
        }

        internal static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        Log.Error("Error: {0}", contextFeature.Error);

                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message = "Internal Server Error"
                        }.ToString());
                    }
                });
            });

            return app;
        }
    }
}