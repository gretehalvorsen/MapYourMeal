using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;

namespace MapYourMeal.Controllers;

public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserController> _logger;

    public UserController(ApplicationDbContext context, ILogger<UserController> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
        {
            _logger.LogInformation("[UserController] Fetching user list from the database.");
            try
            {
                var users = await _context.Users.Include(u => u.Reviews).ToListAsync();
                if(users == null || users.Count == 0)
                {
                _logger.LogWarning("[UserController] No users found in the database.");
                return NotFound("User list not found");
                }
                _logger.LogInformation("[UserController] User list retrieved successfully with {UserCount} users.", users.Count);
                return View(users);
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "[UserController] Error occurred while fetching user list.");
                return StatusCode(500, "An error occurred while fetching the user list.");
            }
        }
}


