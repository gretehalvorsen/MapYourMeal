using MapYourMeal.Models;

namespace MapYourMeal.ViewModels
{
    public class RestaurantViewModel
    {
        public IEnumerable<Restaurant> Restaurants { get; }
        public string? CurrentViewName { get; }

        public RestaurantViewModel(IEnumerable<Restaurant> restaurants, string? currentViewName)
        {
            Restaurants = restaurants;
            CurrentViewName = currentViewName;
        }
    }
}