using System;
using System.Collections.Generic;

namespace MapYourMeal.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();
    }
}