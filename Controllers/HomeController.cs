using Microsoft.AspNetCore.Mvc;

namespace MapYourMeal.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Create()
    {
        return View();
    }

    public IActionResult Delete()
    {
        return View();
    }
}
