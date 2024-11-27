using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext(options)
{
    public new DbSet<User> Users { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Restaurant> Restaurants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);

    // Ensure cascade delete is configured between Restaurant and Review
    modelBuilder.Entity<Review>()
        .HasOne(r => r.Restaurant)
        .WithMany(r => r.Reviews)
        .HasForeignKey(r => r.RestaurantId)
        .OnDelete(DeleteBehavior.Cascade); // Automatically delete all reviews when its restaurant is deleted
}

}
