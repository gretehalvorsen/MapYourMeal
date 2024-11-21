using MapYourMeal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MapYourMeal.DAL
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>?> GetAll();
        Task<IEnumerable<Review>?> GetAllWithUser(); // Method for connecting User details to Reviews
        Task<Review?> GetItemById(int ReviewId);
        Task<bool> Create(Review review);
        Task<bool> Update(Review review);
        Task<bool> Delete(int ReviewId);
    }
}