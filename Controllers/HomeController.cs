using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.DAL;
using System.Linq;

namespace MapYourMeal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Fetch restaurants with average ratings, similar to the SearchResultController
            var restaurants = _context.Restaurants
                .Include(r => r.Reviews)
                .Select(r => new Restaurant
                {
                    RestaurantId = r.RestaurantId,
                    RestaurantName = r.RestaurantName,
                    Longitude = r.Longitude,
                    Latitude = r.Latitude,
                    ImageUrl = r.ImageUrl,
                    Address = r.Address,
                    City = r.City,
                    AverageRating = r.Reviews.Any() ? r.Reviews.Average(rev => rev.Rating) : 0,
                    Reviews = r.Reviews
                })
                .ToList();

            return View(restaurants);  // Pass the data to the view
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
}
