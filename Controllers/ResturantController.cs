using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.ViewModels;
using System.Linq;

namespace MapYourMeal.Controllers
{
    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Restaurant> restaurants = _context.Restaurants.ToList();
            return View(restaurants);
        }
    }
}