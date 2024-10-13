using System;
using System.Collections.Generic;

namespace MapYourMeal.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Note { get; set; }
        public int Rating { get; set; }
        public string? ImageUrl { get; set; }

        public string Dish { get; set; }

        // requirements
        public bool IsGlutenFree { get; set; }
        public bool IsVegan { get; set; }
        public bool IsDairyFree { get; set; }

        // Foreign keys
        public int? UserId { get; set; } // Why need to be ?
        public int? RestaurantId { get; set; } // Why need to be ? 

        public virtual User User { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }
}