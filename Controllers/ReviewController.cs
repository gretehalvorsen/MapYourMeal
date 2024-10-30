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

    public async Task<IActionResult> Table()
    {
        var reviews = await _reviewRepository.GetAll();
        if(reviews == null)
        {
            return NotFound("Review list not found");
        }
        var reviewsViewModel = new ReviewsViewModel(reviews, "Table");
        return View(reviewsViewModel);
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
            return RedirectToAction(nameof(Table));
        }
        return View(review);
    }

    [HttpGet]
    //[Route("Review/Update/{ReviewId}")]
    public async Task<IActionResult> Update(int ReviewId) {
        Console.WriteLine($"Received ReviewId: {ReviewId}");
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review == null)
        {
            Console.WriteLine($"Received ReviewId: {ReviewId}");
            return BadRequest("Review not found for the ReviewId");
        }
        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Review review)
    {
        Console.WriteLine("POST Update method called");
        if (ModelState.IsValid)
        {
            await _reviewRepository.Update(review);
            return RedirectToAction(nameof(Table));
        }
        
        return View(review);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int ReviewId)
    {
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review== null)
        {
            return BadRequest("Review not found for the ReviewId");
        }
        return View(review);
    }

    // POST: Reviews/Delete
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int ReviewId)
    {
        var review = await _reviewRepository.Delete(ReviewId);
        if(review == null)
        {
            return BadRequest("Review deletion failed");
        }
        
        
        return RedirectToAction(nameof(Table));
    }

}

