using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WingsOn.Dal;
using WingsOn.Domain;
using WingsOn.WebAPI.NetCore.Services;

namespace WingsOn.WebAPI.NetCore
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
            services.AddScoped(typeof(IAsyncRepository<Person>), typeof(PersonAsyncRepository));
            services.AddScoped(typeof(IAsyncRepository<Flight>), typeof(FlightAsyncRepository));
            services.AddScoped(typeof(IAsyncRepository<Booking>), typeof(BookingAsyncRepository));

            services.AddScoped(typeof(IPassengerService), typeof(PassengerService));
            services.AddScoped(typeof(IBookingService), typeof(BookingService));
            services.AddScoped(typeof(IFlightService), typeof(FlightService));

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
