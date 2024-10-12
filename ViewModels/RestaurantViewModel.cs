using MapYourMeal.Models;
using System.Collections.Generic;

namespace MapYourMeal.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; }
        public string? CurrentViewName { get; set; }

        public UserViewModel(IEnumerable<User> users, string? currentViewName)
        {
            Users = users;
            CurrentViewName = currentViewName;
        }
    }
}