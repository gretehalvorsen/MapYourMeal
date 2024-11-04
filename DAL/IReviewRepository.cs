using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public interface IReviewRepository
{
    Task<IEnumerable<Review>?> GetAll();
    Task<Review?> GetItemById(int ReviewId);
    Task<bool> Create(Review review);
    Task<bool> Update(Review review);
    Task<bool> Delete(int ReviewId);
}