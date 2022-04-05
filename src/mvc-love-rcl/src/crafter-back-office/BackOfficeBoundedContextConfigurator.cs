using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crafter.BackOffice
{
    public sealed class BackOfficeBoundedContextConfigurator
    {
        private readonly IHostEnvironment environment;

        public BackOfficeBoundedContextConfigurator(IHostEnvironment environment, IServiceCollection services)
        {
            this.environment = environment;
            services.AddControllersWithViews()
                .ConfigureApplicationPartManager(setup => setup.ApplicationParts.Add(new AssemblyPart(this.GetType().Assembly)));
        }

        public void Configure(WebApplication app)
        {
            app.MapAreaControllerRoute(
                name: "BackOffice",
                areaName: "BackOffice",
                pattern: "back-office/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
