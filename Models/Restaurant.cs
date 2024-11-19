using System.ComponentModel.DataAnnotations;
namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{2,20}", ErrorMessage = "The name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Restaurant name")]
        public string RestaurantName { get; set; } = string.Empty;
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Single image URL for use in search results, summaries, etc.
        public string? ImageUrl { get; set; }

        // List of images for use in detailed views
        public List<string>? Images { get; set; } = new List<string>();

        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Webpage { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;

        // Navigation property for reviews
        public virtual List<Review> Reviews{ get; set;} = new List<Review>();
    }
}
