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
            _logger.LogError("[RestaurantRepository] restaurant list not found while executing _restaurantRepository.GetAll()");
            return NotFound("No restaurants found.");
        }
        return Ok(restaurants);
    }

    // GET: Restaurant/Table
    [HttpGet]
    public async Task<IActionResult> Table()
    {
        var restaurants = await _restaurantRepository.GetAll();
        if(restaurants == null)
        {
            _logger.LogError("[RestaurantRepository] restaurant list not found while executing _restaurantRepository.GetAll()");
            return NotFound("Restaurant list not found");
        }
        var restaurantViewModel = new RestaurantViewModel(restaurants, "Table");
        return View(restaurantViewModel);
    }

    public IActionResult Index(int restaurantId)
    {
        var restaurant = _restaurantRepository.GetItemAndReviewsById(restaurantId);
        if (restaurant == null)
        {
            _logger.LogError("[RestaurantController] Restaurant not found when updating the RestaurantId {restaurantId:0000}", restaurantId);
            return NotFound();
        }
        return View(restaurant);
    }

    // GET: Restaurant/Create
    [HttpGet]
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Restaurant/Create
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(Restaurant restaurant, IFormFile? image)
    {
        if (ModelState.IsValid)
        {
            // Saving the image to database
            if (image != null && image.Length > 0)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    restaurant.ImageData = memoryStream.ToArray();
                    restaurant.ImageType = image.ContentType;
                }
            }
            bool returnOk = await _restaurantRepository.Create(restaurant);
            if (returnOk)
                return RedirectToAction("Index", "SearchResult");
        }
        _logger.LogWarning("[RestaurantController] Restaurant creation failed {@restaurant}", restaurant);
        return View(restaurant);
    }

    public async Task<IActionResult> GetImage(int id)
    {
        var restaurant = await _restaurantRepository.GetItemById(id);
        if (restaurant != null && restaurant.ImageData != null)
        {
            return File(restaurant.ImageData, restaurant.ImageType!);
        }
        else
        {
            _logger.LogWarning("Can't find picture");
            return NotFound();
        }
    }

    [HttpGet]
    [Authorize]
    //[Route("Restaurant/Update/{RestaurantId}")]
    public async Task<IActionResult> Update(int RestaurantId) {
        Console.WriteLine($"Received RestaurantId: {RestaurantId}");
        var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
        if(restaurant == null)
        {
            _logger.LogError("[RestaurantController] Restaurant not found when updating the RestaurantId {RestaurantId:0000}", RestaurantId);
            Console.WriteLine($"Received RestaurantId: {RestaurantId}");
            return BadRequest("Restaurant not found for the RestaurantId");
        }
        return View(restaurant);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Update(Restaurant restaurant)
    {
        Console.WriteLine("PUT Update method called");
        if (ModelState.IsValid)
        {
            await _restaurantRepository.Update(restaurant);
            return RedirectToAction(nameof(Table));
        }
        _logger.LogWarning("[RestaurantController] Restaurant update failed {@restaurant}", restaurant);
        return View(restaurant);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Delete(int RestaurantId)
    {
        var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
        if(restaurant== null)
        {
            _logger.LogError("[RestaurantController] Restaurant not found for the RestaurantId {RestaurantId:0000}", RestaurantId);
            return BadRequest("Restaurant not found for the RestaurantId");
        }
        return View(restaurant);
    }

    // POST: Restaurant/Delete
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteConfirmed(int RestaurantId)
    {
        var restaurant = await _restaurantRepository.Delete(RestaurantId);
        if(restaurant == null)
        {
            _logger.LogError("[RestaurantController] Restaurant deletion failed for the RestaurantId {RestaurantId:0000}", RestaurantId);
            return BadRequest("Restaurant deletion failed");
        }
        return RedirectToAction(nameof(Table));
    }
}