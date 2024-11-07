using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; 
using System.Collections.Generic;

namespace MapYourMeal.Models;
public class User : IdentityUser
{
    public User()
    {
        Reviews = new List<Review>();
    }

    // Username and Email are now inherited from IdentityUser
    public int UserId { get; set; }
    
    // Navigation property
    public virtual ICollection<Review> Reviews { get; set; } = default!;
    public User(string userName, string email)
    {
        UserName = userName;
        Email = email;
    }
}