using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAll();
    Task<Review?> GetItemById(int ReviewId);
    Task Create(Review review);
    Task Update(Review review);
    Task<Review?> Delete(int ReviewId);
}