using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using System.Linq;

namespace MapYourMeal.Controllers
{
    public class ReviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var reviews = _context.Reviews.ToList();
            return View(reviews);
        }
    }
}