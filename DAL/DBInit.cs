using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MapYourMeal.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        UserManager<User> userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if(!context.Restaurants.Any())
        {
            var jsonData = System.IO.File.ReadAllText("DAL/infoRestaurants.json");
            var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
            if (restaurants!=null)
            {
                context.AddRange(restaurants);
                context.SaveChanges();
            }
        }

        if(!context.Users.Any())
        {
            var users = new List<User>
            {
                new User
                {
                    UserName = "bob@user.no",
                    Email = "bob@user.no"
                },

                new User
                {
                    UserName = "lisa@user.no",
                    Email = "lisa@user.no"
                }
            };
            foreach (var user in users)
            {
                var result = userManager.CreateAsync(user, "Password123!").Result; // replace "Password123!" with the desired password
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("\n", result.Errors));
                }
            }
            context.SaveChanges();
        }

        if(!context.Reviews.Any())
        {
            var user1 = context.Users.FirstOrDefault(u => u.UserName == "Bob");
            var user2 = context.Users.FirstOrDefault(u => u.UserName == "Lisa");

            var reviews = new List<Review>
            {
                new Review
                {
                    Note = "Den var grei",
                    Rating = 3,
                    ImageUrl = "",
                    Dish = "Pizza",
                    IsGlutenFree = true,
                    IsVegan = true,
                    IsDairyFree = true,
                    CreatedDate = DateTime.Today.AddDays(-3).AddHours(16).AddMinutes(09).AddSeconds(32),
                    RestaurantId = 1,
                    UserId = user1?.Id
                },

                new Review
                {
                    Note = "GOD!",
                    Rating = 5,
                    ImageUrl = "",
                    Dish = "Pasta",
                    IsGlutenFree = true,
                    IsVegan = false,
                    IsDairyFree = false,
                    CreatedDate = DateTime.Today.AddDays(-5).AddHours(10).AddMinutes(28).AddSeconds(02),
                    RestaurantId = 2,
                    UserId = user1?.Id
                },

                new Review
                {
                    Note = "Meh",
                    Rating = 2,
                    ImageUrl = "",
                    Dish = "Hamburger",
                    IsGlutenFree = true,
                    IsVegan = true,
                    IsDairyFree = true,
                    CreatedDate = DateTime.Today.AddDays(-7).AddHours(14).AddMinutes(30).AddSeconds(47),  
                    RestaurantId = 2,
                    UserId = user2?.Id
                }
            };
            context.AddRange(reviews);
            context.SaveChanges();
        }
    }
}