using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using System.Linq;

namespace MapYourMeal.Controllers
{
    public class ItemController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public ItemController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        // GET: /Item/
        public IActionResult Index()
        {
            var restaurants = _appDbContext.Restaurants.ToList();
            return View(restaurants);
        }

        // GET: /Item/Details/{id}
        public IActionResult Details(int id)
        {
            var restaurant = _appDbContext.Restaurants.FirstOrDefault(r => r.RestaurantId == id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return View(restaurant);
        }
    }
}