using System;
using System.Net;
using CalHealth.PatientService.Data;
using CalHealth.PatientService.Messaging.Interfaces;
using CalHealth.PatientService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Serilog;

namespace CalHealth.PatientService.Infrastructure
{
    internal static class ApplicationBuilderExtensions
    {
        private static IAppointmentSubscriber AppointmentSubscriber { get; set; }
        private static IPatientPublisher PatientPublisher { get; set; }

        /// <summary>
        /// Set up the <see cref="AppointmentSubscriber"/> messaging service.
        /// </summary>
        /// <param name="app"></param>
        internal static IApplicationBuilder UseAppointmentSubscriber(this IApplicationBuilder app)
        {
            AppointmentSubscriber = app.ApplicationServices.GetService<IAppointmentSubscriber>();

            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnAppointmentSubscriberStarted);
            lifetime.ApplicationStopping.Register(OnAppointmentSubscriberStopping);

            return app;
        }

        /// <summary>
        /// Register the <see cref="AppointmentSubscriber"/> messaging service on app startup.
        /// </summary>
        private static void OnAppointmentSubscriberStarted()
        {
            AppointmentSubscriber.Register();
        }

        /// <summary>
        /// Deregister the <see cref="AppointmentSubscriber"/> messaging service on app shutdown.
        /// </summary>
        private static void OnAppointmentSubscriberStopping()
        {
            AppointmentSubscriber.Deregister();
        }

        /// <summary>
        /// Set up the <see cref="PatientPublisher"/> messaging service.
        /// </summary>
        /// <param name="app"></param>
        internal static IApplicationBuilder UsePatientPublisher(this IApplicationBuilder app)
        {
            PatientPublisher = app.ApplicationServices.GetService<IPatientPublisher>();

            var lifetime = app.ApplicationServices.GetService<IHostApplicationLifetime>();

            lifetime.ApplicationStarted.Register(OnPatientPublisherStarted);
            lifetime.ApplicationStopping.Register(OnPatientPublisherStopping);

            return app;
        }

        /// <summary>
        /// Register the <see cref="PatientPublisher"/> messaging service on app startup.
        /// </summary>
        private static void OnPatientPublisherStarted()
        {
            PatientPublisher.Register();
        }

        /// <summary>
        /// Deregister the <see cref="PatientPublisher"/> messaging service on app shutdown.
        /// </summary>
        private static void OnPatientPublisherStopping()
        {
            PatientPublisher.Deregister();
        }

        internal static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Californian Health Booking Service API 1.0");
                c.RoutePrefix = string.Empty;
            });

            return app;
        }

        internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            Log.Information("Applying migrations. This may take a moment.");

            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<PatientContext>())
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