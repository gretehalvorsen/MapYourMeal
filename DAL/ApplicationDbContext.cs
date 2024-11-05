using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) // Constructor
    {
        //Database.EnsureCreated(); // Creates an empty database in case it does not exist
    }

    public new DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }
}
