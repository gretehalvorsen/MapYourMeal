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

    public bool ValidateImageType(IFormFile image)
    {
        var allowedTypes = new[] { "image/jpeg", "image/png" };
        return allowedTypes.Contains(image.ContentType);
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
        _logger.LogInformation("[ReviewController] Retrieved {Count} reviews.", reviews.Count());
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
        _logger.LogInformation("[ReviewController] Displaying reviews in table view.");

        return View(reviewsViewModel);
    }

    // GET: Reviews/Create
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create(int restaurantId)
    {
        var restaurant = await _restaurantRepository.GetItemById(restaurantId);
        if(restaurant == null)
        {
            _logger.LogWarning("[ReviewController] Restaurant with ID {RestaurantId} not found.", restaurantId);
            return NotFound("Restaurant not found");
        }            
            var review = new Review
            {
                RestaurantId = restaurantId,
                Restaurant = restaurant, // Add this property to your Review model if it doesn't exist
                UserId = _userManager.GetUserId(User)
            };
            _logger.LogInformation("[ReviewController] Preparing to create review for restaurant {RestaurantId} by user {UserId}.", restaurantId, _userManager.GetUserId(User));
            return View(review);
                     
    }

    // POST: Reviews/Create
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Review review, IFormFile? image)
    {
        if (ModelState.IsValid)
        {
            if (image != null && image.Length > 0)
            {
                // Saving the image to database if it's the correct file type
                if (!ValidateImageType(image))
                {
                    _logger.LogWarning("[ReviewController] Invalid image type {ImageType} provided by user {UserId}.", image.ContentType, _userManager.GetUserId(User));
                    ModelState.AddModelError("Image", "Only JPEG and PNG formats are supported.");
                    return RedirectToAction("Create", new { restaurantId = review.RestaurantId });
                }
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
            {
                 _logger.LogInformation("[ReviewController] Review created successfully for restaurant {RestaurantId} by user {UserId}.", review.RestaurantId, _userManager.GetUserId(User));
                return RedirectToAction("Index","Restaurant", new { restaurantId = review.RestaurantId });
            }
        }
        _logger.LogWarning("[ReviewController] Failed to create review for restaurant {RestaurantId}. {@Review}", review.RestaurantId, review);
        return RedirectToAction("Create", new { restaurantId = review.RestaurantId });
    }

    public async Task<IActionResult> GetImage(int id)
    {
        var review = await _reviewRepository.GetItemById(id);
        if (review != null && review.ImageData != null)
        {
            _logger.LogInformation("[ReviewController] Image retrieved for review {ReviewId}.", id);
            return File(review.ImageData, review.ImageType!);
        }
        else
        {    _logger.LogWarning("[ReviewController] Image not found for review {ReviewId}.", id);
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize]
    //[Route("Review/Update/{ReviewId}")]
    public async Task<IActionResult> Update(int ReviewId) {
        
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review == null)
        {
            _logger.LogError("[ReviewController] Review not found when updating the ReviewId {ReviewId:0000}", ReviewId);
            return BadRequest("Review not found for the ReviewId");
        }
        _logger.LogInformation("[ReviewController] Loading update view for review {ReviewId}.", ReviewId);
        return View(review);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Update(Review review, IFormFile? image)
    {
        if (ModelState.IsValid)
        {
            // Saving the image from form to database
            if (image != null && image.Length > 0)
            {
                if (!ValidateImageType(image))
                {
                    _logger.LogWarning("[ReviewController] Invalid image type {ImageType} for review {ReviewId}.", image.ContentType, review.ReviewId);
                    ModelState.AddModelError("Image", "Only JPEG and PNG formats are supported.");
                    return View(review);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    review.ImageData = memoryStream.ToArray();
                    review.ImageType = image.ContentType;
                }
            }
            else
            {
                //Keep the old image from the database. 
                var existingReview = await _reviewRepository.GetItemById(review.ReviewId);
                if (existingReview != null)
                {
                    review.ImageData = existingReview.ImageData;
                    review.ImageType = existingReview.ImageType;
                }
            }
            bool returnOk = await _reviewRepository.Update(review);
            if(returnOk)
            {
                if(User.IsInRole("Admin"))
                    {
                        _logger.LogInformation("[ReviewController] Review {ReviewId} updated successfully by admin.", review.ReviewId);
                        return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
                    }
                    else
                    {
                        _logger.LogInformation("[ReviewController] Review {ReviewId} updated successfully by user.", review.ReviewId);
                        return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
                    }
            }
        }
        _logger.LogWarning("[ReviewController] Failed to update review {ReviewId}. {@Review}", review.ReviewId, review);
        return View(review);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int ReviewId)
    {
        var review = await _reviewRepository.GetItemById(ReviewId);
        if(review== null)
        {
             _logger.LogError("[ReviewController] Review with ID {ReviewId} not found for deletion.", ReviewId);
            return NotFound("Review not found for the ReviewId");
        }
         _logger.LogInformation("[ReviewController] Preparing to delete review {ReviewId}.", ReviewId);
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
            _logger.LogError("[ReviewController] Failed to delete review {ReviewId}.", ReviewId);
            return BadRequest("Review deletion failed");
        }
        if(User.IsInRole("Admin"))
        {
             _logger.LogInformation("[ReviewController] Review {ReviewId} deleted successfully.", ReviewId);
            return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
        }
        else
        {
             _logger.LogInformation("[ReviewController] User is not admin. Deletion not authorized for Review {ReviewId}.", ReviewId);
            return RedirectToPage("/Account/Manage/Index", new { area = "Identity" });
        }
    }
}