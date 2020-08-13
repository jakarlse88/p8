using AutoMapper;
using CalHealth.PatientService.Infrastructure;
using CalHealth.PatientService.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CalHealth.PatientService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson(options =>
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services
                .ConfigureDbContext(Configuration)
                .AddOptionsObjects(Configuration)
                .AddRepositoryLayer()
                .AddServiceLayer()
                .ConfigureCors()
                .AddHostedService<AppointmentSubscriber>()
                .ConfigureSwagger()
                .AddAutoMapper(typeof(Startup));
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
                .UseAuthorization()
                .UseSwaggerUI()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });
        }
    }
}
