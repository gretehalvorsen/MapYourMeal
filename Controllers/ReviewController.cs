using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.DAL;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.ViewModels;

namespace MapYourMeal.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(IReviewRepository reviewRepository, ILogger<ReviewController> logger)
    {
        _reviewRepository = reviewRepository;
        _logger = logger;
    }

    public async Task<IActionResult> Table()
    {
        var reviews = await _reviewRepository.GetAll();
        if(reviews == null)
        {
            _logger.LogError("[ReviewRepository] review list not found while executing _reviewRepository.GetAll()");
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
            bool returnOk = await _reviewRepository.Create(review);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ReviewController] Review creation failed {@review}", review);
        return View(review);
    }

    [HttpGet]
    //[Route("Review/Update/{ReviewId}")]
    public async Task<IActionResult> Update(int ReviewId) {
        //Console.WriteLine($"Received ReviewId: {ReviewId}");
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review == null)
        {
            _logger.LogError("[ReviewController] Review not found when updating the ReviewId {ReviewId:0000}", ReviewId);
            return BadRequest("Review not found for the ReviewId");
        }
        return View(review);
    }

    [HttpPost]
    public async Task<IActionResult> Update(Review review)
    {
        //Console.WriteLine("POST Update method called");
        if (ModelState.IsValid)
        {
            bool returnOk = await _reviewRepository.Update(review);
            if(returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ReviewController] Review update failed {@review}", review);
        return View(review);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int ReviewId)
    {
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review== null)
        {
            _logger.LogError("[ReviewController] Review not found for the ReviewId {ReviewId:0000}", ReviewId);
            return BadRequest("Review not found for the ReviewId");
        }
        return View(review);
    }

    // POST: Reviews/Delete
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int ReviewId)
    {
        bool returnOk = await _reviewRepository.Delete(ReviewId);
        if(!returnOk)
        {
            _logger.LogError("[ReviewController] Review deletion failed for the ReviewId {ReviewId:0000}", ReviewId);
            return BadRequest("Review deletion failed");
        }
        
        
        return RedirectToAction(nameof(Table));
    }

}

