// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using MapYourMeal.DAL;
using MapYourMeal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
namespace MapYourMeal.Areas.Identity.Pages.Account.Manage
{
   
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger; 

        public IndexModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context,
            ILogger<IndexModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _logger = logger;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        public IList<Review> UserReviews { get; set;}

        // New property for restaurant list
        public IList<Restaurant> Restaurants { get; set; }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };

            UserReviews = _context.Reviews
            .Where(r => r.UserId == user.Id)
            .ToList();

            // Load all restaurants
            Restaurants = await _context.Restaurants.ToListAsync();
            var allReviews = await _context.Reviews.ToListAsync(); //testing
            ViewData["AllReviews"] = allReviews;
        }

       
        public async Task<IActionResult> OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            
             _logger.LogInformation("Current User ID: {UserId}", userId);

            // Check if the user is authenticated before proceeding
            if (!User.Identity.IsAuthenticated)
            {
                _logger.LogWarning("User not authenticated. Redirecting to login page.");
                // Redirect to the login page if the user is not logged in
                return RedirectToPage("/Account/Login");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                _logger.LogError("User not found. Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            _logger.LogInformation("User found: {UserId}, loading user data.", user.Id);

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                 _logger.LogError("User NOT found. Unable to load user with ID '{UserId}'.", _userManager.GetUserId(User));
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid for user {UserId}. Reloading the page.", user.Id);
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                _logger.LogInformation("Phone number changed for user {UserId}. Updating phone number.", user.Id);
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    _logger.LogError("Unexpected error when trying to set phone number for user {UserId}.", user.Id);
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User {UserId} profile updated successfully.", user.Id);

            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
            
        }

        // New method for deleting restaurant
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var restaurant = await _context.Restaurants.FindAsync(id);
            if (restaurant != null)
            {
                _logger.LogInformation("Deleting restaurant with ID {RestaurantId}.", id);
                _context.Restaurants.Remove(restaurant);
                await _context.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Restaurant with ID {RestaurantId} not found.", id);
            }
            return RedirectToPage();
        }

        // New method for updating restaurant - to be implemented
        public IActionResult OnPostUpdateAsync(int id)
        {
            // code to update the restaurant
            _logger.LogInformation("Updating restaurant with ID {RestaurantId}.", id);
            // you might need to accept more parameters depending on what fields of the restaurant you want to update
            return RedirectToPage();
        }
    }
}