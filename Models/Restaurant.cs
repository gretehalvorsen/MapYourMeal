namespace MapYourMeal.Models;
public class Restaurant
{
    public int RestaurantId { get; set; }
    public string RestaurantName { get; set; } = string.Empty;
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    // navigation property
    public virtual List<Review> Reviews { get; set; } = default!; // Later change to "= new List<Review>(); ???
}
