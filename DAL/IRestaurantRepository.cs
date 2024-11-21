using MapYourMeal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapYourMeal.DAL
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>?> GetAll();
        Task<Restaurant?> GetItemById(int RestaurantId);
        Task<bool> Create(Restaurant restaurant);
        Task<bool> Update(Restaurant restaurant);
        Task<bool> Delete(int RestaurantId);
        Restaurant GetItemAndReviewsAndUsersById(int RestaurantId); // Updated method
    }
}