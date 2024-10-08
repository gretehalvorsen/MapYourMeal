using System.Collections.Generic;

namespace MapYourMeal.Models
{
    public class User
    {
        public User()
        {
            Reviews = new List<Review>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        // Navigation property
        public ICollection<Review> Reviews { get; set; }

        public User(string userName, string email)
        {
            UserName = userName;
            Email = email;
        }
    }
}