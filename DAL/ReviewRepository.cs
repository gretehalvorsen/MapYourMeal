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

    /*public async Task<Review?> GetItemById(int id)
    {
    var review = await _db.Reviews.FindAsync(id);
    if (review == null)
    {
        // sjekke ID-en
        Console.WriteLine($"Review with ID {id} not found.");
    }
    return review;
}*/

    public async Task<Review?> GetItemById(int ReviewId)
    {
        return await _db.Reviews.FindAsync(ReviewId); // Kontroller at id eksisterer i databasen
    }


    public async Task Create(Review review)
    {
        _db.Reviews.Add(review);
        await _db.SaveChangesAsync();
    }
    
    public async Task Update(Review review)
    {
        _db.Reviews.Update(review);
        await _db.SaveChangesAsync();
    
    }

    public async Task<Review?> Delete(int ReviewId)
    {
        var review = await _db.Reviews.FindAsync(ReviewId);
        if (review == null)
        {
            return null;
        }

        _db.Reviews.Remove(review);
        await _db.SaveChangesAsync();
        return review;
    }
}