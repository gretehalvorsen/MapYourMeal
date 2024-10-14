using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using MapYourMeal.ViewModels;

namespace MapYourMeal.Models;
public class ReviewController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReviewController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Reviews/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Reviews/Create
    [HttpPost]
    public IActionResult Create(Review review)
    {
        Console.WriteLine(review);
        if (ModelState.IsValid)
        {
            _context.Add(review);
            _context.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
        return View(review);
    }
}
