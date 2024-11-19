using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using MapYourMeal.DAL;

namespace MapYourMeal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("[HomeController] Starting restaurant search in Index method.");
            try
            {
    
                // Fetch restaurants and calculate average rating directly in the query
                var restaurantList = _context.Restaurants
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
                        // Calculate average rating, or default to 0 if no reviews
                        AverageRating = r.Reviews.Any() ? r.Reviews.Average(rev => rev.Rating) : 0,
                        Reviews = r.Reviews
                    })
                    .ToList();
                if (restaurantList == null || !restaurantList.Any())
                {
                    _logger.LogWarning("[HomeController] No restaurants found in search results.");
                    return NotFound("No restaurants found.");
                }

                _logger.LogInformation("[HomeController] Search completed successfully with {Count} restaurants found.", restaurantList.Count);
                return View(restaurantList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[HomeController] An error occurred while fetching restaurant data.");
                return View("Error");
            }
        }
    }

}        
