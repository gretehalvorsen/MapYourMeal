using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;
using MapYourMeal.DAL;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace MapYourMeal.Controllers;

public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<ReviewController> _logger;

    private readonly UserManager<User> _userManager;

   public ReviewController(IReviewRepository reviewRepository, IRestaurantRepository restaurantRepository, ILogger<ReviewController> logger, UserManager<User> userManager)
    {
        _reviewRepository = reviewRepository;
        _restaurantRepository = restaurantRepository;
        _logger = logger;
        _userManager = userManager;
    }

    [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {            
            var reviews = await _reviewRepository.GetAllWithUser();
            if(reviews == null)
            {
                _logger.LogError("[ReviewRepository] review list not found while executing _reviewRepository.GetAllWithUser()");
                return NotFound("Review list not found");
            }
            return Ok(reviews);
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
    [Authorize]
    public async Task<IActionResult> Create(int restaurantId)
    {
        var restaurant = await _restaurantRepository.GetItemById(restaurantId);
        if (restaurant != null)
        {
            var review = new Review
            {
                RestaurantId = restaurantId,
                Restaurant = restaurant, // Add this property to your Review model if it doesn't exist
                UserId = _userManager.GetUserId(User)
            };
            return View(review);
        }
         else
        {
            return NotFound(); 
        }
    }

    // POST: Reviews/Create
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Review review, IFormFile? image)
    {
        if (ModelState.IsValid)
        {
            // Saving the image to database
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    review.ImageData = memoryStream.ToArray();
                    review.ImageType = image.ContentType;
                }
            }
            // Saving the review to the database
            bool returnOk = await _reviewRepository.Create(review);
            if (returnOk)
                return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[ReviewController] Review creation failed {@review}", review);
        return View(review);
    }

    public async Task<IActionResult> GetImage(int id)
    {
        var review = await _reviewRepository.GetItemById(id);
        if (review != null && review.ImageData != null)
        {
            return File(review.ImageData, review.ImageType!);
        }
        else
        {
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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
    [Authorize]
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

