using MapYourMeal.Models;
using System.Collections.Generic;

namespace MapYourMeal.ViewModels
{
    public class ItemsViewModel
    {
        public IEnumerable<Restaurant> Restaurants;
        public string? CurrentViewName;

        public ItemsViewModel(IEnumerable<Restaurant> restaurants, string? currentViewName)
        {
            Restaurants = restaurants;
            CurrentViewName = currentViewName;
        }
    }
}