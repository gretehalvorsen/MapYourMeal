using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MapYourMeal.DAL;
using MapYourMeal.ViewModels;
using MapYourMeal.Models;

namespace MapYourMeal.Controllers;


public class RestaurantController : Controller
    {
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IRestaurantRepository restaurantRepository)
        {
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        public IActionResult GetAllRestaurants()
        {
            var restaurants = _restaurantRepository.GetAll();
            return Ok(restaurants);
        }

        // GET: Restaurant/Table
        [HttpGet]
        public async Task<IActionResult> Table()
        {
            var restaurants = await _restaurantRepository.GetAll();
            if(restaurants == null)
            {
                return NotFound("Restaurant list not found");
            }
            var restaurantViewModel = new RestaurantViewModel(restaurants, "Table");
            return View(restaurantViewModel);
        }

        public async Task<IActionResult> Index(int restaurantId)
        {
            var restaurant = await _restaurantRepository.GetItemById(restaurantId);
            if (restaurant == null)
            {
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
        public async Task<IActionResult> Create(Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                bool returnOk = await _restaurantRepository.Create(restaurant);
                if (returnOk)
                    return RedirectToAction("Index", "SearchResult");
            }
            return View(restaurant);
        }

        [HttpGet]
        [Authorize]
        //[Route("Restaurant/Update/{RestaurantId}")]
        public async Task<IActionResult> Update(int RestaurantId) {
            Console.WriteLine($"Received RestaurantId: {RestaurantId}");
            var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
            if(restaurant == null)
            {
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
            return View(restaurant);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int RestaurantId)
        {
            var restaurant = await _restaurantRepository.GetItemById(RestaurantId);
            if(restaurant== null)
            {
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
                return BadRequest("Restaurant deletion failed");
            }
            return RedirectToAction(nameof(Table));
        }
    }