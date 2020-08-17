using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapper;
using CalHealth.BookingService.Infrastructure.Extensions;
using CalHealth.BookingService.Messaging;

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
                .AddLazyCache()
                .AddOptionsObjects(Configuration)
                .ConfigureDbContext(Configuration)
                .AddRepositoryLayer()
                .AddServiceLayer(Configuration)
                .AddHostedService<PatientSubscriber>()
                .AddAutoMapper(typeof(Startup))
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
                .UseCustomExceptionHandler()
                .UseRouting()
                .UseCors()
                .UseAuthorization()
                .UseSwaggerUI()
                .UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}