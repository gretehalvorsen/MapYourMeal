namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public double AverageRating { get; set; }

        // Single image URL for use in search results, summaries, etc.
        public string? ImageUrl { get; set; }

        // List of images for use in detailed views
        public List<string>? Images { get; set; } = new List<string>();

        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;

        // Navigation property for reviews
        public ICollection<Review> Reviews { get; set; }
    }
}
