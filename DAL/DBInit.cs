using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MapYourMeal.DAL;

public static class DBInit
{
    public static async Task Seed(IApplicationBuilder app, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        using var serviceScope = app.ApplicationServices.CreateScope();
        ApplicationDbContext context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Seed roles if they don't exist
        if (await roleManager.FindByNameAsync("Admin") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (await roleManager.FindByNameAsync("User") == null)
        {
            await roleManager.CreateAsync(new IdentityRole("User"));
        }

        // Seed Admin user if it doesn't exist
        var adminUser = await userManager.FindByEmailAsync("admin@admin.no");
        if (adminUser == null)
        {
            var newAdminUser = new User { UserName = "admin@admin.no", Email = "admin@admin.no" };
            var result = await userManager.CreateAsync(newAdminUser, "Admin@1234");  // Set the password for Admin
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdminUser, "Admin");  // Assign Admin role to the new admin user
            }
        }

        // Always create Bob and Lisa with unique emails
        var bobUser = await userManager.FindByEmailAsync("bob@user.no");
        if (bobUser == null)
        {
            bobUser = new User { UserName = "bob@user.no", Email = "bob@user.no" };  // Unique email for Bob
            var resultBob = await userManager.CreateAsync(bobUser, "BobUser@1234"); //Set the password for Bob
            if (resultBob.Succeeded)
            {
                Console.WriteLine("Bob user successfully created");
                await userManager.AddToRoleAsync(bobUser, "User");  // Assign User role to Bob
            }
            else{
                foreach (var error in resultBob.Errors)
                {
                    Console.WriteLine($"Error creating Bob user: {error.Description}");
                }
            }
        }

        var lisaUser = await userManager.FindByEmailAsync("lisa@user.no");  // Unique email for Lisa
        if (lisaUser == null)
        {
            lisaUser = new User { UserName = "lisa@user.no", Email = "lisa@user.no" };  // Unique email for Lisa
            var resultLisa = await userManager.CreateAsync(lisaUser, "Lisa@1234"); // Set the password for Lisa
            if (resultLisa.Succeeded)
            {
                await userManager.AddToRoleAsync(lisaUser, "User");  // Assign User role to Lisa
            }
        }

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
            var user1 = context.Users.FirstOrDefault(u => u.UserName == "bob@user.no");
            var user2 = context.Users.FirstOrDefault(u => u.UserName == "lisa@user.no");

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
                    UserId = user1?.Id
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
                    UserId = user1?.Id
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
                    UserId = user2?.Id
                }
            };
            context.AddRange(reviews);
            context.SaveChanges();
        }
    }
}