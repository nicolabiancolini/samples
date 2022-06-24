// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Crafter.BackOffice
{
    public class BackOfficeStartup : IModuleStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseEndpoints(builder =>
            {
                builder.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                builder.MapFallbackToAreaPage(
                    area: "BackOffice",
                    page: "/_IngredientsSelection",
                    pattern: $"~/back-office/ingredients-selection/{FallbackEndpointRouteBuilderExtensions.DefaultPattern}");

                builder.MapFallbackToAreaPage(
                    area: "BackOffice",
                    page: "/_RecipeComposition",
                    pattern: $"~/back-office/recipe-composition/{FallbackEndpointRouteBuilderExtensions.DefaultPattern}");

                builder.MapBlazorHub("~/back-office/_blazor");
            });
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services;
        }
    }
}
