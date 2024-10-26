using Microsoft.EntityFrameworkCore;

namespace MapYourMeal.Models;

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
            var restaurants = new List<Restaurant>
            {
                new Restaurant
                {
                    RestaurantName = "La Luna",
                    Longitude = 12.0,
                    Latitude = 12.0
                },

                new Restaurant
                {
                    RestaurantName = "Egon",
                    Longitude = 47.0,
                    Latitude = 74.0
                },

                new Restaurant
                {
                    RestaurantName = "La Luna",
                    Longitude = 78.0,
                    Latitude = 45.0
                }
            };
            context.AddRange(restaurants);
            context.SaveChanges();
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
                    ImageUrl = "",
                    Dish = "Pizza",
                    IsGlutenFree = true,
                    IsVegan = true,
                    IsDairyFree = true,
                    RestaurantId = 1,
                    UserId = 1
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
                    RestaurantId = 2,
                    UserId = 1
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
                    RestaurantId = 2,
                    UserId = 2
                }
            };
            context.AddRange(reviews);
            context.SaveChanges();
        }
    }
}