using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MapYourMeal.Controllers
{
    public class SearchResultController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SearchResultController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
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

            return View(searchResult);
        }
    }
}
