// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Crafter;
public interface IModuleStartup
{
    void Configure(IApplicationBuilder app);

    IServiceCollection ConfigureServices(IServiceCollection services);
}
