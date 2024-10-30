namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double AverageRating { get; set; }

        // Single image URL for use in search results, summaries, etc.
        public string? ImageUrl { get; set; }

        // List of images for use in detailed views
        public List<string>? Images { get; set; } = new List<string>();

        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;

        // Navigation property for reviews
        public virtual List<Review> Reviews{ get; set;} = default!;
    }
}
