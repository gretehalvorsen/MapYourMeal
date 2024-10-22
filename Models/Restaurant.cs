namespace MapYourMeal.Models;

public class Restaurant
{
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public double AverageRating { get; set;}
    public string? ImageUrl { get; set; }
    public string? Address { get; set; } = string.Empty;
    public string? City { get; set; } = string.Empty;

    // Navigation property for reviews
    public ICollection<Review> Reviews { get; set; }
}
