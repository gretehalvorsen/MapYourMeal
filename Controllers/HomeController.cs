using Microsoft.AspNetCore.Mvc;

namespace MapYourMeal.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Create
        public IActionResult Create()
        {
            return View();
        }
    }
}