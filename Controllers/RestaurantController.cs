using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MapYourMeal.DAL;
using MapYourMeal.ViewModels;
using MapYourMeal.Models;

namespace MapYourMeal.Controllers;


public class RestaurantController : Controller
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly ILogger<RestaurantController> _logger;

    public RestaurantController(IRestaurantRepository restaurantRepository, ILogger<RestaurantController> logger)
    {
        _restaurantRepository = restaurantRepository;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetAllRestaurants()
    {            
        var restaurants = _restaurantRepository.GetAll();
        if(restaurants == null)
        {
            _logger.LogError("[RestaurantController] restaurant list not found while executing _restaurantRepository.GetAll()");
            return NotFound("No restaurants found.");
        }
        _logger.LogInformation("[RestaurantController] Fetched all restaurants successfully.");
        return Ok(restaurants);
    }

    public IActionResult Index(int restaurantId)
    {
        var restaurant = _restaurantRepository.GetItemAndReviewsAndUsersById(restaurantId); // Updated method to 
        if (restaurant == null)
        {
            _logger.LogError("[RestaurantController] Restaurant not found when updating the RestaurantId {restaurantId:0000}", restaurantId);
            return NotFound();
        }
        _logger.LogInformation("[RestaurantController] Loaded details for restaurant with ID {RestaurantId}.", restaurantId);
        return View(restaurant);
    }

    // GET: Restaurant/Create
    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        _logger.LogInformation("[RestaurantController] Accessing Create Restaurant page.");
        return View();
    }

    // POST: Restaurant/Create
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Restaurant restaurant, IFormFile? image)
    {
        _logger.LogInformation("[RestaurantController] Attempting to create restaurant: {@Restaurant}.", restaurant);

        if (ModelState.IsValid)
        {
            // Checking if an image has been uploaded
            if (image != null && image.Length > 0)
            {
                _logger.LogInformation("[RestaurantController] Image uploaded for restaurant: {RestaurantId}. Image type: {ImageType}", 
                restaurant.RestaurantId, image.ContentType);

                // Validate the image format
                var allowedTypes = new[] { "image/jpeg", "image/png" };
                if(!allowedTypes.Contains(image.ContentType))
                {
                     _logger.LogWarning("[RestaurantController] Invalid image format for restaurant: {RestaurantId}. Provided image type: {ImageType}.", 
                    restaurant.RestaurantId, image.ContentType);
                    ModelState.AddModelError("Image", "Only JPEG and PNG formats are supported.");
                    return View(restaurant);
                }
                //Process the image and save to the restaurant object
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    restaurant.ImageData = memoryStream.ToArray();
                    restaurant.ImageType = image.ContentType;
                    _logger.LogInformation("[RestaurantController] Image successfully processed for restaurant: {RestaurantId}.", restaurant.RestaurantId);
                }
            }
            // Try to create the restaurant in the database
            bool returnOk = await _restaurantRepository.Create(restaurant);
            if (returnOk)
            {   
                 _logger.LogInformation("[RestaurantController] Restaurant created successfully: {RestaurantId}.", restaurant.RestaurantId);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogError("[RestaurantController] Failed to create restaurant: {RestaurantId}.", restaurant.RestaurantId);
            }
        }
        else
        {   
            _logger.LogWarning("[RestaurantController] ModelState is invalid for restaurant creation. Restaurant: {@Restaurant}.", restaurant);
        }
        // Return the view with the current data if creation fails or ModelState is invalid
        return View(restaurant);
    }

    public async Task<IActionResult> GetImage(int id)
    {
        _logger.LogInformation("[RestaurantController] Attempting to retrive image for restaurant: {@Restaurant}.", id);

        var restaurant = await _restaurantRepository.GetItemById(id);
        if (restaurant != null && restaurant.ImageData != null)
        {
            _logger.LogInformation("[RestaurantController] Serving image for restaurant with ID {RestaurantId}.", id);
            return File(restaurant.ImageData, restaurant.ImageType!);
        }
        else
        {
            _logger.LogWarning("[RestaurantController] Image not found for restaurant with ID {RestaurantId}.", id);
            return NotFound();
        }
    }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        //[Route("Restaurant/Update/{RestaurantId}")]
        public async Task<IActionResult> Update(int RestaurantId) 
        {
            var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
            if(restaurant == null)
            {
                _logger.LogError("[RestaurantController] Restaurant with ID {RestaurantId} not found attempting to access update page", RestaurantId);
                return BadRequest("Restaurant not found for the RestaurantId");
            }
             _logger.LogInformation("[RestaurantController] Accessing Update page for restaurant with ID {RestaurantId}.", RestaurantId);
            return View(restaurant);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Restaurant restaurant, IFormFile? image)
        {
            _logger.LogInformation("[RestaurantController] POST Update method called for Restaurant ID: {RestaurantId}", restaurant.RestaurantId);
            Console.WriteLine("POST Update method called");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("[RestaurantController] Attempting to update restaurant with ID {RestaurantId}.", restaurant.RestaurantId);
                // Saving the image from form to database
            if (image != null && image.Length > 0)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png" };
                if(!allowedTypes.Contains(image.ContentType))
                {
                    _logger.LogWarning("[RestaurantController] Unsupported image format provided for restaurant ID {RestaurantId}.", restaurant.RestaurantId);
                    ModelState.AddModelError("Image", "Only JPEG and PNG formats are supported.");
                    return View(restaurant);
                }
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    restaurant.ImageData = memoryStream.ToArray();
                    restaurant.ImageType = image.ContentType;
                     _logger.LogInformation("[RestaurantController] Image uploaded and saved for Restaurant ID: {RestaurantId}", restaurant.RestaurantId);
                }
            }
            else
            {
                //Keep the old image from the database. 
                var existingRestaurant = await _restaurantRepository.GetItemById(restaurant.RestaurantId);
                if (existingRestaurant != null)
                {
                    restaurant.ImageData = existingRestaurant.ImageData;
                    restaurant.ImageType = existingRestaurant.ImageType;
                    _logger.LogInformation("[RestaurantController] No new image uploaded. Using existing image for Restaurant ID: {RestaurantId}", restaurant.RestaurantId);
                }
            }
                bool returnOk = await _restaurantRepository.Update(restaurant);
                if(returnOk)
                {
                    _logger.LogInformation("[RestaurantController] Successfully updated restaurant with ID {RestaurantId}.", restaurant.RestaurantId);
                    return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
                }
            }
            _logger.LogWarning("[RestaurantController] Restaurant update failed for ID: {RestaurantId}", restaurant.RestaurantId);
            return View(restaurant);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int RestaurantId)
        {
            var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
            if(restaurant== null)
            {
                _logger.LogError("[RestaurantController] Restaurant not found for deletion with ID {RestaurantId}.", RestaurantId);
                return BadRequest("Restaurant not found for the RestaurantId");
            }
            _logger.LogInformation("[RestaurantController] Accessing Delete page for restaurant with ID {RestaurantId}.", RestaurantId);
            return View(restaurant);
        }

        // POST: Restaurant/Delete
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int RestaurantId)
        {
            var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
            if (restaurant == null)
            {
                _logger.LogWarning("[RestaurantController] Attempt to delete a non-existing restaurant with ID {RestaurantId}.", RestaurantId);
                return NotFound("Restaurant not found for the provided ID.");
            }

            // Ensure cascading delete is handled automatically by EntityFramework
            bool returnOk = await _restaurantRepository.Delete(RestaurantId);

            if (!returnOk)
            {
                _logger.LogError("[RestaurantController] Restaurant deletion failed for the RestaurantId {RestaurantId:0000}", RestaurantId);
                return BadRequest("Restaurant deletion failed.");
            }

            _logger.LogInformation("[RestaurantController] Restaurant with ID {RestaurantId:0000} deleted successfully.", RestaurantId);
            // Redirecting to the admin page after successful deletion
            return RedirectToPage("/Account/Manage/Admin", new { area = "Identity" });
        }
    }