// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Crafter.IngredientsSelection
{
    public class IngredientsSelectionStartup : IModuleStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            // Method intentionally left empty.
            app.UseEndpoints(builder =>
            {
                builder.MapBlazorHub("~/IngredientsSelection/_blazor");
            });
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            return services;
        }
    }
}
