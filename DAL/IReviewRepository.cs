using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAll();
    Task<Review?> GetItemById(int id);
    Task Create(Review review);
}