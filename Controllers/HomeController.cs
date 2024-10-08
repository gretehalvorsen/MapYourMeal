using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var restaurants = _context.Restaurants.Include(r => r.Reviews).ToList();
        return View(restaurants);
    }
}