using System;
using System.Net;
using CalHealth.BookingService.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace CalHealth.BookingService.Infrastructure.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
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
            Console.WriteLine("Applying migrations. This may take a moment.");

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
                    Console.WriteLine("A fatal error occurred while trying to apply migrations: {ex}", ex);
                    throw;
                }
            }

            Console.WriteLine("Migrations operation successful. Continuing application Startup.");
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
                        Console.WriteLine("Error: {0}", contextFeature.Error);

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