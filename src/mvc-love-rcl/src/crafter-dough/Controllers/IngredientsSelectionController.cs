using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crafter.Dough.WebApp.Models;

namespace Crafter.Dough.WebApp.Controllers;

[Route("ingredients-selection")]
public class IngredientsSelectionController : Controller
{
    private readonly ILogger<IngredientsSelectionController> _logger;

    public IngredientsSelectionController(ILogger<IngredientsSelectionController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View("_Host");
    }
}
