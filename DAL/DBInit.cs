using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using System.Text.Json;

namespace MapYourMeal.DAL;

public static class DBInit
{
    public static void Seed(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if(!context.Restaurants.Any())
        {
            var jsonData = System.IO.File.ReadAllText("DAL/infoRestaurants.json");
            var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
            if (restaurants!=null){
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
                    UserName = "Bob",
                    Email = "bob@user.no"
                },

                new User
                {
                    UserName = "Lisa",
                    Email = "lisa@user.no"
                }
            };
            context.AddRange(users);
            context.SaveChanges();
        }

        if(!context.Reviews.Any())
        {
            var reviews = new List<Review>
            {
                new Review
                {
                    Note = "Den var grei",
                    Rating = 3,
                    Dish = "Pizza",
                    IsGlutenFree = true,
                    IsVegan = true,
                    IsDairyFree = true,
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/Egon.png"),
                    ImageType = "image/png",
                    CreatedDate = DateTime.Today.AddDays(-3).AddHours(16).AddMinutes(09).AddSeconds(32),
                    RestaurantId = 1,
                    UserId = 1
                },

                new Review
                {
                    Note = "GOD!",
                    Rating = 5,
                    Dish = "Pasta",
                    IsGlutenFree = true,
                    IsVegan = false,
                    IsDairyFree = false,
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/pizza2.png"),
                    ImageType = "image/png",
                    CreatedDate = DateTime.Today.AddDays(-5).AddHours(10).AddMinutes(28).AddSeconds(02),
                    RestaurantId = 2,
                    UserId = 1
                },

                new Review
                {
                    Note = "Meh",
                    Rating = 2,
                    Dish = "Hamburger",
                    IsGlutenFree = true,
                    IsVegan = true,
                    IsDairyFree = true,
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/Egon.png"),
                    ImageType = "image/png",
                    CreatedDate = DateTime.Today.AddDays(-7).AddHours(14).AddMinutes(30).AddSeconds(47),  // 7 days ago at 2:30 PM,
                    RestaurantId = 2,
                    UserId = 2
                }
            };
            context.AddRange(reviews);
            context.SaveChanges();
        }
    }
}