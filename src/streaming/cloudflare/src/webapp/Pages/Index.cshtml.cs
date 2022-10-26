// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Streaming.Cloudflare.WebApp.Services.Streaming;
using Streaming.Cloudflare.WebApp.Services.Streaming.Models;

namespace Streaming.Cloudflare.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> logger;
    private readonly CloudflareStream cloudflareStream;

    public IndexModel(ILogger<IndexModel> logger, CloudflareStream cloudflareStream)
    {
        this.logger = logger;
        this.cloudflareStream = cloudflareStream;
        this.Videos = Array.Empty<Video>();
    }

    [BindProperty]
    public Video[] Videos { get; private set; }

    public async Task OnGetAsync(string? search)
    {
        this.Videos = (await this.cloudflareStream.ListVideosAsync(search)).ToArray();
    }

    public async Task<IActionResult> OnPostFileAsync(string name, IFormFile file)
    {
        await this.cloudflareStream.UploadFileAsync(name, await BinaryData.FromStreamAsync(file.OpenReadStream()));
        return this.RedirectToPage("/Index");
    }

    public async Task<IActionResult> OnPostCopyAsync(string name, Uri url, [FromForm(Name = "require-signed-urls")] bool requireSignedURLs, [FromForm(Name = "allowed-origins")] string allowedOrigins)
    {
        await this.cloudflareStream.CopyVideoAsync(name, url, requireSignedURLs, this.TranslateAllowedOriginsInput(allowedOrigins));
        return this.RedirectToPage("/Index");
    }

    public IActionResult OnPostSearch(string search)
    {
        return this.RedirectToPage("/Index", new { search = search });
    }

    private IReadOnlyCollection<Uri> TranslateAllowedOriginsInput(string allowedOrigins)
    {
        return (allowedOrigins?.Split(new char[] { ';', ',' }) ?? Array.Empty<string>()).Select(allowedOrigin => new Uri(allowedOrigin)).ToList().AsReadOnly();
    }
}
