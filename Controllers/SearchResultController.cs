using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using MapYourMeal.DAL;

namespace MapYourMeal.Controllers
{
    public class SearchResultController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SearchResultController> _logger;

        public SearchResultController(ApplicationDbContext context, ILogger<SearchResultController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("[SearchResultController] Starting restaurant search in Index method.");
            try
            {
    
                // Fetch restaurants and calculate average rating directly in the query
                var searchResult = _context.Restaurants
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
                if (searchResult == null || !searchResult.Any())
                {
                    _logger.LogWarning("[SearchResultController] No restaurants found in search results.");
                    return NotFound("No restaurants found.");
                }

                _logger.LogInformation("[SearchResultController] Search completed successfully with {Count} restaurants found.", searchResult.Count);
                return View(searchResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SearchResultController] An error occurred while fetching restaurant data.");
                return View("Error");
            }
        }
    }

}        
