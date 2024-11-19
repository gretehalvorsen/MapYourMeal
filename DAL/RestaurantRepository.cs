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

    public async Task<IEnumerable<Restaurant>> GetAll()
    {
        return await _db.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetItemById(int RestaurantId)
    {
        return await _db.Restaurants.FindAsync(RestaurantId); // Kontroller at id eksisterer i databasen
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
    
    public async Task<bool> Update(Restaurant restaurant)
    {
        try
        {
        _db.Restaurants.Update(restaurant);
        await _db.SaveChangesAsync();
        return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RestaurantRepository] review FindAsync(RestaurantId) failed when updating the RestaurantId {RestaurantId: 0000}, error message: {e}", restaurant, e.Message);
            return false;
        }
    
    }

    public async Task<bool> Delete(int RestaurantId)
    {
        try
        {
        var restaurant = await _db.Restaurants.FindAsync(RestaurantId);
        if (restaurant == null)
        {
            _logger.LogError("[RestaurantRepository] restaurant not found for the RestaurantId {RestaurantId: 0000}", RestaurantId);
            return false;
        }
        _db.Restaurants.Remove(restaurant);
        await _db.SaveChangesAsync();
        return true;
        }
        catch(Exception e)
        {
            _logger.LogError("[RestaurantRepository] restaurant deletion failed for the RestaurantId {RestaurantId: 0000}, error message: {e}", RestaurantId, e.Message);
            return false;
        }
    }
}