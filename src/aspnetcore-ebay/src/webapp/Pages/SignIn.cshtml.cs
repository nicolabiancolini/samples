using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetCore.LoremIpsum.Pages
{
    public class SignInModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnGetSignIn(string provider, string returnUrl = null)
        {
            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl ?? "/" }, provider);
        }
    }
}
