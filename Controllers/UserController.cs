using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.ViewModels;
using System.Linq;

namespace MapYourMeal.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<User> users = _context.Users.ToList();
            return View(users);
        }
    }
}