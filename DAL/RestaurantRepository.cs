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
            return await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.RestaurantId == RestaurantId); // Kontroller at id eksisterer i databasen
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

    public async Task<bool> Delete(int restaurantId)
    {
        try
        {
        // Find the restaurant including its reviews (if cascade delete is set up, reviews will be deleted automatically)
            var restaurant = await _db.Restaurants
                                    .Include(r => r.Reviews) // Explicitly include reviews (although cascade delete should handle this)
                                    .FirstOrDefaultAsync(r => r.RestaurantId == restaurantId);
            
            if (restaurant == null)
            {
                _logger.LogError("[RestaurantRepository] Restaurant not found for the RestaurantId {RestaurantId:0000}", restaurantId);
                return false;
            }

            _db.Restaurants.Remove(restaurant); // Remove the restaurant (reviews will be deleted because of cascade delete)
            await _db.SaveChangesAsync(); // Save the changes to the database

            _logger.LogInformation("[RestaurantRepository] Restaurant with ID {RestaurantId:0000} deleted successfully.", restaurantId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError("[RestaurantRepository] Restaurant deletion failed for the RestaurantId {RestaurantId:0000}, error message: {ErrorMessage}", restaurantId, e.Message);
            return false;
        }
    }


    public Restaurant GetItemAndReviewsAndUsersById(int RestaurantId) // Updated method name
    {
        return _db.Restaurants
            .Include(r => r.Reviews)
                .ThenInclude(review => review.User) // Include User data
            .FirstOrDefault(r => r.RestaurantId == RestaurantId) 
            ?? throw new NullReferenceException();
    }

    public Restaurant GetItemAndReviewsById(int RestaurantId)
        {
            return _db.Restaurants
                .Include(r => r.Reviews)
                    .ThenInclude(review => review.User) // Include User data
                .FirstOrDefault(r => r.RestaurantId == RestaurantId) 
                ?? throw new NullReferenceException();
        }
}