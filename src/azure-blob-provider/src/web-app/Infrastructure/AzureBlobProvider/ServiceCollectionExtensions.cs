// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApp.Infrastructure.AzureBlobProvider;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureBlobProvider(this IServiceCollection services, Action<BlobStorageOptions> options)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            services.AddOptions();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddSingleton<BlobProvider>();
            services.Configure(options);
            return services;
        }
    }
}
