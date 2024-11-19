using MapYourMeal.Models;

namespace MapYourMeal.DAL;
public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>?> GetAll();
    Task<Restaurant?> GetItemById(int RestaurantId);
    Task<bool> Create(Restaurant restaurant);
    Task Update(Restaurant restaurant);
    Task<Restaurant?> Delete(int RestaurantId);
    Restaurant GetItemAndReviewsById(int RestaurantId);
}