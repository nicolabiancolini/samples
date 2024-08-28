// See the LICENSE.TXT file in the project root for full license information.

namespace Streaming.Cloudflare.WebApp.Services.Streaming
{
    public class CloudflareStreamOptions
    {
        public static readonly string SectionName = "Cloudflare";

        public string AccountId { get; set; } = default!;

        public string AccessToken { get; set; } = default!;
    }
}
