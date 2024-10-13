using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

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
            // Eagerly load the Reviews
            List<User> users = _context.Users
                .Include(u => u.Reviews)
                .ToList();
            return View(users);
        }
    }
}