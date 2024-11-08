using MapYourMeal.Models;

namespace MapYourMeal.DAL;
public interface IRestaurantRepository
{
    Task<IEnumerable<Restaurant>> GetAll();
    Task<Restaurant?> GetItemById(int RestaurantId);
    Task<bool> Create(Restaurant restaurant);
    //Remove Update and Delete? Only have the option to create and read?
    Task Update(Restaurant restaurant);
    Task<Restaurant?> Delete(int RestaurantId);
}