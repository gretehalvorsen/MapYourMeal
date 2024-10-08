using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MapYourMeal.Models;

public class RestaurantsController : Controller
{
    private readonly AppDbContext context;

    public RestaurantsController(AppDbContext context)
    {
        this.context = context;
    }

    public IActionResult Details(int id)
    {
        var restaurant = context.Restaurants.Include(r => r.Reviews).FirstOrDefault(r => r.RestaurantId == id);

        if (restaurant == null)
        {
            return NotFound();
        }

        return View(restaurant);
    }

}