using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.DAL;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.ViewModels;

namespace MapYourMeal.Controllers;
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewController(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
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
        if (ModelState.IsValid)
        {
            await _reviewRepository.Create(review);
            return RedirectToAction(nameof(Create));
        }
        return View(review);
    }
}
