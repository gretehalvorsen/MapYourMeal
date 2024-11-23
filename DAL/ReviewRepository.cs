using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;

public class ReviewRepository : IReviewRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<ReviewRepository> _logger;

    public ReviewRepository(ApplicationDbContext db, ILogger<ReviewRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Review>?> GetAll()
    {
        try
        {
            return await _db.Reviews.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] reviews ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<IEnumerable<Review>?> GetAllWithUser()
        {
            try
            {
                return await _db.Reviews
                    .Include(r => r.User)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("[ReviewRepository] reviews with users ToListAsync() failed when GetAllWithUser(), error message: {e}", e.Message);
                return null;
            }
        }

    public async Task<Review?> GetItemById(int ReviewId)
    {
        try
        {
            return await _db.Reviews.FindAsync(ReviewId); // Kontroller at id eksisterer i databasen
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review FindAsync() failed when GetItemById() for ReviewId {ReviewID:0000}, error message: {e}", ReviewId, e.Message);
            return null;
        }
       
    }


    public async Task<bool> Create(Review review)
    {
        try
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review creation failed for review {@review}, error message: {e}", review, e.Message);
            return false;
        }
    }
    
    public async Task<bool> Update(Review review)
    {
        try
        {
            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review FindAsync(ReviewId) failed when updating the ReviewId {ReviewId: 0000}, error message: {e}", review, e.Message);
            return false;
        }
    }
    public async Task<bool> Delete(int ReviewId)
    {
        try
        {
            var review = await _db.Reviews.FindAsync(ReviewId);
            if (review == null)
            {
                _logger.LogError("[ReviewRepository] review not found for the ReviewId {ReviewId: 0000}", ReviewId);
                return false;
            }

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review deletion failed for the ReviewId {ReviewId: 0000}, error message: {e}", ReviewId, e.Message);
            return false;
        }
    }
}