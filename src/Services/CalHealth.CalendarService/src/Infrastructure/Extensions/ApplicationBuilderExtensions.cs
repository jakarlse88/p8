using System;
using System.Net;
using CalHealth.CalendarService.Data;
using CalHealth.CalendarService.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Serilog;

namespace CalHealth.CalendarService.Infrastructure
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            Log.Information("Applying migrations. This may take a moment.");

            using (var serviceScope = app.ApplicationServices.CreateScope())
            using (var context = serviceScope.ServiceProvider.GetRequiredService<CalendarContext>())
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