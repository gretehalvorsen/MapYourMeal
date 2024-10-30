namespace MapYourMeal.Models;
public class User
{
    public User()
    {
        Reviews = new List<Review>();
    }

    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    // Navigation property
    public virtual ICollection<Review> Reviews { get; set; } = default!;
    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }
}