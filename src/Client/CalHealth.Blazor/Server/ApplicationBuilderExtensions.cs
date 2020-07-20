using CalHealth.Blazor.Server.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalHealth.Blazor.Server
{
    internal static class ApplicationBuilderExtensions
    {
        private static IAppointmentSubscriber AppointmentSubscriber { get; set; }
        
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
    }
}