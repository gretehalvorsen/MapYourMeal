using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MapYourMeal.Controllers;
public class RestaurantController : Controller
{
    private readonly ApplicationDbContext _context;

    public RestaurantController(ApplicationDbContext context)
    {
        _context = context;
    }

 
    public IActionResult Index()
{
    List<Restaurant> restaurants = _context.Restaurants.Include(r => r.Reviews).ToList();

    foreach (var restaurant in restaurants)
    {
        if (restaurant.Reviews != null && restaurant.Reviews.Any())
        {
            restaurant.AverageRating = restaurant.Reviews.Average(r => r.Rating);
        }
        else
        {
            restaurant.AverageRating = 0; // Default value when no reviews are present
        }
    }

    return View(restaurants);
}
}