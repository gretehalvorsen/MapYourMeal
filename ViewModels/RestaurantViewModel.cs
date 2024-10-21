using MapYourMeal.Models;
using System.Collections.Generic;

namespace MapYourMeal.ViewModels
{
    public class RestaurantViewModel
    {
        public IEnumerable<Restaurant> Restaurants;
        public string? CurrentViewName;

        public RestaurantViewModel(IEnumerable<Restaurant> restaurants, string? currentViewName)
        {
            Restaurants = restaurants;
            CurrentViewName = currentViewName;
        }
    }
}