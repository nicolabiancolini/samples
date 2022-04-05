// See the LICENSE.TXT file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Crafter.RecipeComposition.Areas.RecipeComposition.BackOffice.Controllers;

[BoundedContextArea("BackOffice")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> logger;

    public HomeController(ILogger<HomeController> logger)
    {
        this.logger = logger;
    }

    public IActionResult Index()
    {
        return this.View("_Host");
    }
}
