using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;
public class RestaurantRepository : IRestaurantRepository
{
    private readonly ApplicationDbContext _db;
    private readonly ILogger<RestaurantRepository> _logger;

    public RestaurantRepository(ApplicationDbContext db, ILogger<RestaurantRepository> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IEnumerable<Restaurant>?> GetAll()
    {
        try
        {
            return await _db.Restaurants.ToListAsync();
        }
        catch (Exception e)
        {
            _logger.LogWarning("[RestaurantRepository] restaurant ToListAsync() failed when GetAll(), error message: {e}", e.Message);
            return null;
        }
    }

    public async Task<Restaurant?> GetItemById(int RestaurantId)
    {
        try
        {
            return await _db.Restaurants.FindAsync(RestaurantId); // Kontroller at id eksisterer i databasen
        }
        catch (Exception e)
        {
            _logger.LogWarning("[RestaurantRepository] restaurant FindAsync() failed when GetItemById() for RestaurantId {RestaurantID:0000}, error message: {e}", RestaurantId, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Restaurant restaurant)
    {
        try{
            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: " + e);
            return false;
        }
    }
    
    public async Task Update(Restaurant restaurant)
    {
        _db.Restaurants.Update(restaurant);
        await _db.SaveChangesAsync();
    
    }

    public async Task<Restaurant?> Delete(int RestaurantId)
    {
        var restaurant = await _db.Restaurants.FindAsync(RestaurantId);
        if (restaurant == null)
        {
            return null;
        }
        _db.Restaurants.Remove(restaurant);
        await _db.SaveChangesAsync();
        return restaurant;
    }

    public Restaurant GetItemAndReviewsById(int RestaurantId)
    {
        return _db.Restaurants
        .Include(r => r.Reviews)
        .FirstOrDefault(r => r.RestaurantId == RestaurantId) 
        ?? throw new NullReferenceException();
    }
}