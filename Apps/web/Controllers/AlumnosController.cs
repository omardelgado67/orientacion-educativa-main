using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class AlumnosController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}