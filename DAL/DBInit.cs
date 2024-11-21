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
        //context.Database.EnsureDeleted();
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
                Console.WriteLine($"Bob user created with ID: {bobUser.Id}");
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
            /*var jsonData = System.IO.File.ReadAllText("DAL/infoRestaurants.json");
            var restaurants = JsonSerializer.Deserialize<List<Restaurant>>(jsonData);
            if (restaurants!=null)
            {
                context.AddRange(restaurants);
                context.SaveChanges();
            }*/

            var restaurants = new List<Restaurant>
            {
                new Restaurant
                { 
                    RestaurantName = "Dattera til Hagen",
                    Longitude = 10.760130402815815,
                    Latitude = 59.913304499999995,
                    Address = "Grønland, 0188 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "http://www.dattera.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Dattera_til_Hagen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Lorry",
                    Longitude = 10.728849,
                    Latitude = 59.9207598,
                    Address = "Hegdehaugsveien, 0350 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.lorry.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Lorry.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Grand Café",
                    Longitude = 10.7393822,
                    Latitude = 59.9137595,
                    Address = "Karl Johans gate, 0026 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://grand.no/no/Restauranter--Barer/Grand-Cafe/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Grand_Cafe.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Asylet",
                    Longitude = 10.762362,
                    Latitude = 59.912872,
                    Address = "Grønland, 0188 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.asylet.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Asylet.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Theatercaféen",
                    Longitude = 10.7340202,
                    Latitude = 59.9139731,
                    Address = "Klingenberggata, 0161 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.theatercafeen.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Theatercafeen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Statholdergaarden",
                    Longitude = 10.743218,
                    Latitude = 59.909517,
                    Address = "Kirkegata, 0153 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.statholdergaarden.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Statholdergaarden.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Maaemo",
                    Longitude = 10.7581789,
                    Latitude = 59.907552,
                    Address = "0191 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://maaemo.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Maaemo.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Olympen",
                    Longitude = 10.764388,
                    Latitude = 59.912256,
                    Address = "Grønlandsleiret, 0190 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.olympen.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Olympen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Mathallen",
                    Longitude = 10.75202993636007,
                    Latitude = 59.92211835,
                    Address = "Gerd Kjølaas' plass, 0178 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://mathallenoslo.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Olympen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Baker Hansen",
                    Longitude = 10.68447,
                    Latitude = 59.919816,
                    Address = "Karenslyst allé, 0278 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.bakerhansen.no/sjolyst/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Baker_Hansen3.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Stortorvets Gjæstgiveri",
                    Longitude = 10.745070134493664,
                    Latitude = 59.9135515,
                    Address = "Grensen, 0159 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.stortorvet.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Stortorvets_Gjaestgiveri.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Beer Palace",
                    Longitude = 10.726195,
                    Latitude = 59.910515,
                    Address = "Holmens gate, 0250 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://beerpalace.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Beer_Palace.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Mahayana Asian Dining",
                    Longitude = 10.736849,
                    Latitude = 59.9133121,
                    Address = "Stortingsgata, 0162 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://mahayana.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Mahayana_Asian_Dining.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Kampen Bistro",
                    Longitude = 10.781102,
                    Latitude = 59.913764,
                    Address = "Nittedalgata, 0654 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "http://www.kampenbistro.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Kampen_Bistro.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Ni&Tyve",
                    Longitude = 10.727298249999999,
                    Latitude = 59.92042075,
                    Address = "Parkveien, 0352 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "http://niogtyve.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/NiOgTyve.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Villa Paradiso Grünerløkka",
                    Longitude = 10.757423,
                    Latitude = 59.923502,
                    Address = "Grüners gate, 0554 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://www.villaparadiso.no/restauranter/grunerlokka/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Villa_Paradiso_Grunerlokka.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Kaffebrenneriet",
                    Longitude = 10.758756,
                    Latitude = 59.923489,
                    Address = "Grüners gate, 0554 Oslo, Norway",
                    City = "Oslo",
                    Webpage = "https://kaffebrenneriet.no/butikkene/thorvald-meyers-gate-55-olaf-ryes-plass/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Kaffebrenneriet.png"),
                    ImageType = "image/png",
                }
            };
            context.AddRange(restaurants);
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