using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Razor;
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
            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.AreaViewLocationFormats.Clear();
                options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Areas/{2}/Shared/Views/{1}/{0}.cshtml");
                options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
            });
        }

        public void Configure(WebApplication app)
        {
            app.MapAreaControllerRoute(
                name: "BackOffice",
                areaName: "BackOffice",
                pattern: "back-office/{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "BackOffice/IngredientsSelection",
                areaName: "BackOffice/IngredientsSelection",
                pattern: "back-office/ingredients-selection/{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "BackOffice/RecipeComposition",
                areaName: "BackOffice/RecipeComposition",
                pattern: "back-office/recipe-composition/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
