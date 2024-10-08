using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MapYourMeal.Models;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });

        SeedData(app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppDbContext>());
    }



    // Adding demo data to db if there is no data
    private static void SeedData(AppDbContext context)
    {
        if (!context.Restaurants.Any())
        {
            var restaurant1 = new Restaurant { RestaurantName = "Demo Restaurant 1", Longitude = 10.7519, Latitude = 59.9139 };
            var restaurant2 = new Restaurant { RestaurantName = "Demo Restaurant 2", Longitude = 10.7522, Latitude = 59.9138 };

            context.Restaurants.AddRange(restaurant1, restaurant2);
            context.SaveChanges(); // Save the restaurants

            var user = new User { UserName = "Demo User", Email = "demo_user@example.com" };
            context.Users.Add(user);
            context.SaveChanges(); // Save the user

            var review = new Review
            {
                Dish = "Pizza",
                Note = "Great gluten-free options!",
                Rating = 5,
                ImageUrl = null,
                IsGlutenFree = true,
                UserId = user.UserId,
                RestaurantId = restaurant1.RestaurantId
            };

            context.Reviews.Add(review);
            context.SaveChanges();
        }
    }
}