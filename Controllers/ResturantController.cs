using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

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
            // Eagerly load the Reviews
            List<Restaurant> restaurants = _context.Restaurants
                .Include(r => r.Reviews)
                .ToList();
            return View(restaurants);
        }
    }
}