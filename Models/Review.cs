namespace MapYourMeal.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Note { get; set; }
        public int Rating { get; set; }
        public string? ImageUrl { get; set; }

        public string Dish { get; set; }

        // recuerments
        public bool IsGlutenFree { get; set; }

        // Foreign keys
        public int UserId { get; set; }
        public int RestaurantId { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}