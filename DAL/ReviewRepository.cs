using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;
using MapYourMeal.ViewModels;

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
        _logger.LogInformation("[ReviewRepository] Retrieving all reviews.");
        try
        {
            var reviews = await _db.Reviews.ToListAsync();
            _logger.LogInformation("[ReviewRepository] Successfully retrieved {Count} reviews.", reviews.Count);
            return reviews;
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
                var reviews = await _db.Reviews
                    .Include(r => r.User)
                    .ToListAsync();
                    _logger.LogInformation("[ReviewRepository] Successfully retrieved {Count} reviews with user data.", reviews.Count);
                    return reviews;
            }
            catch (Exception e)
            {
                _logger.LogError("[ReviewRepository] reviews with users ToListAsync() failed when GetAllWithUser(), error message: {e}", e.Message);
                return null;
            }
        }

    public async Task<Review?> GetItemById(int ReviewId)
    {    _logger.LogInformation("[ReviewRepository] Retrieving review with ID {ReviewId}.", ReviewId);
        try
        {
            var review = await _db.Reviews.AsNoTracking().FirstOrDefaultAsync(r => r.ReviewId == ReviewId); // Kontroller at id eksisterer i databasen
            if (review==null)
            {
                _logger.LogWarning("[ReviewRepository] No review found with ID {ReviewId}.", ReviewId);

            }
            else
            {
                _logger.LogInformation("[ReviewRepository] Successfully retrieved review with ID {ReviewId}.", ReviewId);

            }
            return review;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review FindAsync() failed when GetItemById() for ReviewId {ReviewID:0000}, error message: {e}", ReviewId, e.Message);
            return null;
        }
       
    }


    public async Task<bool> Create(Review review)
    {   
        _logger.LogInformation("[ReviewRepository] Creating a new review: {@Review}.", review);
        try
        {
            _db.Reviews.Add(review);
            await _db.SaveChangesAsync();
            _logger.LogInformation("[ReviewRepository] Successfully created review with ID {ReviewId}.", review.ReviewId);
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
        _logger.LogInformation("[ReviewRepository] Updating review with ID {ReviewId}.", review.ReviewId);
        try
        {
            _db.Reviews.Update(review);
            await _db.SaveChangesAsync();
            _logger.LogInformation("[ReviewRepository] Successfully updated review with ID {ReviewId}.", review.ReviewId);
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
        _logger.LogInformation("[ReviewRepository] Attempting to delete review with ID {ReviewId}.", ReviewId);
        try
        {
            var review = await _db.Reviews.FindAsync(ReviewId);
            if (review == null)
            {
                _logger.LogWarning("[ReviewRepository] review not found for the ReviewId {ReviewId: 0000}", ReviewId);
                return false;
            }

            _db.Reviews.Remove(review);
            await _db.SaveChangesAsync();
            _logger.LogInformation("[ReviewRepository] Successfully deleted review with ID {ReviewId}.", ReviewId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[ReviewRepository] review deletion failed for the ReviewId {ReviewId: 0000}, error message: {e}", ReviewId, e.Message);
            return false;
        }
    }
}