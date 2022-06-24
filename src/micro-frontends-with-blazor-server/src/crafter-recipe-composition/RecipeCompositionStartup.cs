// See the LICENSE.TXT file in the project root for full license information.

using Crafter.RecipeComposition.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Crafter.RecipeComposition
{
    public class RecipeCompositionStartup : IModuleStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            // Method intentionally left empty.
        }

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<RecipeService>();
            return services;
        }
    }
}
