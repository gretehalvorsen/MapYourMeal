using System.ComponentModel.DataAnnotations;
namespace MapYourMeal.Models;
public class Review
{
    public int ReviewId { get; set; }
    [StringLength(200)]
    public string? Note { get; set; }

    [Range(1, 10, ErrorMessage = "The rating must be between 1 and 10.")]
    public int Rating { get; set; }
    public string? ImageUrl { get; set; }
    [StringLength(40)]

    
    public string Dish { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    // requirements
    public bool IsGlutenFree { get; set; }
    public bool IsVegan { get; set; }
    public bool IsDairyFree { get; set; }

    // Foreign keys
    public string? UserId { get; set; }
    public int? RestaurantId { get; set; }
    
    // navigation properties
    public virtual User? User { get; set; }
    public virtual Restaurant? Restaurant { get; set; }
}
