using System.ComponentModel.DataAnnotations;
namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        
        [RegularExpression(@"[0-9a-zA-ZæøåÆØÅ .\-]{2,40}", ErrorMessage = "The name must be numbers or letters and between 2 to 40 characters.")]
        [Display(Name = "Restaurant name")]
        public string RestaurantName { get; set; } = string.Empty;
        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude must be between -180 and 180.")]
        public double Longitude { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Latitude must be between -180 and 180.")]
        public double Latitude { get; set; }
        public double AverageRating { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public byte[]? ImageData { get; set; } // For new user images
        public string? ImageType { get; set; } // Checking image type (png, jpeg)

        // List of images for use in detailed views
        public List<string>? Images { get; set; } = new List<string>();

        [Required]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string PostalCode { get; set; } = string.Empty;
        [Required]
        public string City { get; set; } = string.Empty;
        public string? Webpage { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;

        // Navigation property for reviews
        public virtual List<Review> Reviews{ get; set;} = new List<Review>();
    }
}
