using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCore.LoremIpsum.Pages
{
    public class SignInModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
#nullable enable
        public string? Provider { get; set; }
#nullable restore

        public IActionResult OnGet(string returnUrl = null)
        {
            if (this.Provider is not null)
            {
                return Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = returnUrl ?? "/",
                        IsPersistent = HttpContext.Features.Get<ITrackingConsentFeature>()?.CanTrack ?? false
                    },
                    this.Provider!);
            }

            return this.Page();
        }
    }
}
