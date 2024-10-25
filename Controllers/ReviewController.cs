using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using MapYourMeal.ViewModels;

namespace MapYourMeal.Controllers;
public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReviewController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reviews/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Reviews/Create
    [HttpPost]
    public async Task<IActionResult> Create(Review review)
    {
        Console.WriteLine("Before if-statement");
        if (ModelState.IsValid)
        {
            Console.WriteLine("Inside if-statement with review: " + review);
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Create));
        }
        return View(review);
    }
}
