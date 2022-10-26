// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Streaming.Cloudflare.WebApp.Services.Streaming;
using Streaming.Cloudflare.WebApp.Services.Streaming.Models;

namespace Streaming.Cloudflare.WebApp.Pages;

public class PlayerModel : PageModel
{
    private readonly CloudflareStream cloudflareStream;

    public PlayerModel(CloudflareStream cloudflareStream)
    {
        this.Video = default!;
        this.cloudflareStream = cloudflareStream;
    }

    [BindProperty]
    public Video Video { get; private set; }

    public async Task OnGetAsync(string uid)
    {
        this.Video = await this.cloudflareStream.RetrieveVideoDetailsAsync(uid);
    }

    public async Task<IActionResult> OnPostUpdateAsync(string name, [FromForm(Name = "require-signed-urls")] bool requireSignedURLs, [FromForm(Name = "allowed-origins")] string allowedOrigins)
    {
        var uid = this.RouteData.Values["uid"] !.ToString() !;
        await this.cloudflareStream.UpdateVideoMetadataAsync(
            uid,
            name,
            requireSignedURLs,
            this.TranslateAllowedOriginsInput(allowedOrigins));

        return this.RedirectToPage("/Player", new { uid = uid });
    }

    private IReadOnlyCollection<Uri> TranslateAllowedOriginsInput(string allowedOrigins)
    {
        return (allowedOrigins?.Split(new char[] { ';', ',' }) ?? Array.Empty<string>()).Select(allowedOrigin => new Uri(allowedOrigin, UriKind.RelativeOrAbsolute)).ToList().AsReadOnly();
    }
}
