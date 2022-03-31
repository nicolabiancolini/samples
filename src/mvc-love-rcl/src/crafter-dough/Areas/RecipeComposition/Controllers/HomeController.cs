using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crafter.Dough.WebApp.Models;

namespace Crafter.Dough.WebApp.Areas.RazorClassLib2.Controllers;

[Area("RecipeComposition")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("_Host");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
