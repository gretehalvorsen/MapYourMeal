using System.Collections.Generic;

namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public Restaurant()
        {
            Reviews = new List<Review>();
        }

        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        // Navigation property
        public ICollection<Review> Reviews { get; set; }
    }
}