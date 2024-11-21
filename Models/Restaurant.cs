using System.ComponentModel.DataAnnotations;
namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{2,20}", ErrorMessage = "The name must be numbers or letters and between 2 to 20 characters.")]
        [Display(Name = "Restaurant name")]
        public string RestaurantName { get; set; } = string.Empty;
        [Required]
        [Range(-90, 90, ErrorMessage = "Longitude must be between -90 and 90.")]
        public double Longitude { get; set; }
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude must be between -90 and 90.")]
        public double Latitude { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public byte[]? ImageData { get; set; } // For new user images
        public string? ImageType { get; set; } // Checking image type (png, jpeg)

        // List of images for use in detailed views
        public List<string>? Images { get; set; } = new List<string>();

        public string? Address { get; set; } = string.Empty;
        public string? PostalCode { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Webpage { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;

        // Navigation property for reviews
        public virtual List<Review> Reviews{ get; set;} = new List<Review>();
    }
}
