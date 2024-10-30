using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.DAL;

namespace MapYourMeal.Controllers;


    public class RestaurantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // This action fetches a specific restaurant by its ID
        public IActionResult Index(int restaurantId)
        {
            // Fetch the restaurant along with its reviews
            var restaurant = _context.Restaurants
                .Include(r => r.Reviews)  // Include reviews if necessary
                .FirstOrDefault(r => r.RestaurantId == restaurantId);  // Get the restaurant with the given ID

            if (restaurant == null)
            {
                return NotFound();  // If no restaurant is found, return 404
            }

            return View(restaurant);  // Pass the single restaurant object to the view
        }
    }