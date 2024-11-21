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
                    Longitude = 10.76020,
                    Latitude = 59.91342,
                    Address = "Grønland 10",
                    PostalCode = "0188",
                    City = "Oslo, Norway",
                    Webpage = "http://www.dattera.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Dattera_til_Hagen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Lorry",
                    Longitude = 10.72875,
                    Latitude = 59.92098,
                    Address = "Parkveien 12",
                    PostalCode = "0350",
                    City = "Oslo, Norway",
                    Webpage = "https://www.lorry.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Lorry.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Grand Café",
                    Longitude = 10.73900,
                    Latitude = 59.91398,
                    Address = "Karl Johans gt. 31",
                    PostalCode = "0026",
                    City = "Oslo, Norway",
                    Webpage = "https://grand.no/no/Restauranter--Barer/Grand-Cafe/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Grand_Cafe.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Asylet",
                    Longitude = 10.76230,
                    Latitude = 59.91297,
                    Address = "Grønland 28",
                    PostalCode = "0188",
                    City = "Oslo, Norway",
                    Webpage = "https://www.asylet.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Asylet.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Theatercaféen",
                    Longitude = 10.73396,
                    Latitude = 59.91414,
                    Address = "Stortingsagta 24-26",
                    PostalCode = "0117",
                    City = "Oslo, Norway",
                    Webpage = "https://www.theatercafeen.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Theatercafeen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Statholdergaarden",
                    Longitude = 10.74319,
                    Latitude = 59.90957,
                    Address = "Råshusgata 11",
                    PostalCode = "0151",
                    City = "Oslo, Norway",
                    Webpage = "https://www.statholdergaarden.no",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Statholdergaarden.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Maaemo",
                    Longitude = 10.75823,
                    Latitude = 59.90761,
                    Address = "Dronning Eufemias gate 23",
                    PostalCode = "0194",
                    City = "Oslo, Norway",
                    Webpage = "https://maaemo.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Maaemo.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Olympen",
                    Longitude = 10.76451,
                    Latitude = 59.91240,
                    Address = "Grønlandsleiret 15",
                    PostalCode = "0190",
                    City = "Oslo, Norway",
                    Webpage = "https://www.olympen.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Olympen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Mathallen",
                    Longitude = 10.75163,
                    Latitude = 59.92244,
                    Address = "Vulkan 5",
                    PostalCode = "0178",
                    City = "Oslo, Norway",
                    Webpage = "https://mathallenoslo.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Olympen.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Baker Hansen Skøyen",
                    Longitude = 10.68224,
                    Latitude = 59.92064,
                    Address = "Sjølyst plass 1, 3",
                    PostalCode = "0278",
                    City = "Oslo, Norway",
                    Webpage = "https://www.bakerhansen.no/sjolyst/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Baker_Hansen3.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Stortorvets Gjæstgiveri",
                    Longitude = 10.74477,
                    Latitude = 59.91347,
                    Address = "Grensen 1",
                    PostalCode = "0159",
                    City = "Oslo, Norway",
                    Webpage = "https://www.stortorvet.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Stortorvets_Gjaestgiveri.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Beer Palace",
                    Longitude = 10.72614,
                    Latitude = 59.91065,
                    Address = "Holmens gate 3",
                    PostalCode = "0250",
                    City = "Oslo, Norway",
                    Webpage = "https://beerpalace.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Beer_Palace.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Mahayana Asian Dining",
                    Longitude = 10.73685,
                    Latitude = 59.91334,
                    Address = "Stortingsgata 12",
                    PostalCode = "0161",
                    City = "Oslo, Norway",
                    Webpage = "https://mahayana.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Mahayana_Asian_Dining.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Kampen Bistro",
                    Longitude = 10.78117,
                    Latitude = 59.91399,
                    Address = "Bøgata 21",
                    PostalCode = "0655",
                    City = "Oslo, Norway",
                    Webpage = "http://www.kampenbistro.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Kampen_Bistro.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Ni&Tyve",
                    Longitude = 10.72740,
                    Latitude = 59.92050,
                    Address = "Parkveien 29",
                    PostalCode = "0350",
                    City = "Oslo, Norway",
                    Webpage = "http://niogtyve.no/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/NiOgTyve.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Villa Paradiso Grünerløkka",
                    Longitude = 10.75755,
                    Latitude = 59.92393,
                    Address = "Olaf Ryes plass 8",
                    PostalCode = "0552",
                    City = "Oslo, Norway",
                    Webpage = "https://www.villaparadiso.no/restauranter/grunerlokka/",
                    ImageData = System.IO.File.ReadAllBytes("wwwroot/images/POI/Villa_Paradiso_Grunerlokka.png"),
                    ImageType = "image/png",
                },

                new Restaurant
                {
                    RestaurantName = "Kaffebrenneriet",
                    Longitude = 10.75871,
                    Latitude = 59.92396,
                    Address = "Thorvald Meyers gate 55",
                    PostalCode = "0555",
                    City = "Oslo, Norway",
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