using CalHealth.BookingService.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using CalHealth.BookingService.Infrastructure.Extensions;

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
            services.AddControllers();

            services
                .AddAutoMapper(typeof(Startup))
                .ConfigureDbContext(Configuration)
                .AddRepositoryLayer()
                .AddServiceLayer()
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