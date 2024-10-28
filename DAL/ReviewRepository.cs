using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _db;

    public ReviewRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Review>> GetAll()
    {
        return await _db.Reviews.ToListAsync();
    }

    public async Task<Review?> GetItemById(int id)
    {
        return await _db.Reviews.FindAsync(id);
    }

    public async Task Create(Review review)
    {
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
    }
}