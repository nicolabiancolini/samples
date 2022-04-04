using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Crafter.IngredientsSelection
{
    public sealed class IngredientsSelectionBoundedContextConfigurator
    {
        private readonly IHostEnvironment environment;

        public IngredientsSelectionBoundedContextConfigurator(IHostEnvironment environment, IServiceCollection services)
        {
            this.environment = environment;
            services.AddControllersWithViews()
                .ConfigureApplicationPartManager(setup => setup.ApplicationParts.Add(new AssemblyPart(this.GetType().Assembly)));
        }

        public void Configure(WebApplication app)
        {
            app.MapAreaControllerRoute(
                name: "IngredientsSelection",
                areaName: "IngredientsSelection",
                pattern: $"{RoutePrefix.CustomerFacing.Value.Trim('/')}/{{controller=Home}}/{{action=Index}}/{{id?}}");

            app.MapAreaControllerRoute(
                name: "BackOffice/IngredientsSelection",
                areaName: "BackOffice/IngredientsSelection",
                pattern: $"{RoutePrefix.BackOffice.Value.Trim('/')}/{{controller=Home}}/{{action=Index}}/{{id?}}");
        }
    }
}
