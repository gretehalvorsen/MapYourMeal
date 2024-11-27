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
            _logger.LogInformation("[RestaurantRepository] Retrieving all restaurants.");
            var restaurants = await _db.Restaurants.ToListAsync();
             _logger.LogInformation("[RestaurantRepository] Successfully retrieved {Count} restaurants.", restaurants.Count);
            return restaurants;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "[RestaurantRepository] Error occurred while retrieving all restaurants. Message: {Message}", e.Message);
            return null;
        }
    }

    public async Task<Restaurant?> GetItemById(int RestaurantId)
    {
        try
        {
            _logger.LogInformation("[RestaurantRepository] Attempting to retrieve restaurant with ID {RestaurantId}.", RestaurantId);
            var restaurant = await _db.Restaurants.AsNoTracking().FirstOrDefaultAsync(r => r.RestaurantId == RestaurantId); // Kontroller at id eksisterer i databasen
            if (restaurant==null)
            {
                _logger.LogWarning("[RestaurantRepository] No restaurant found with ID {RestaurantId}.", RestaurantId);
            }
            else
            {   
                _logger.LogInformation("[RestaurantRepository] Successfully retrieved restaurant with ID {RestaurantId}.", RestaurantId);
            }
            return restaurant;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[RestaurantRepository] Error occurred while retrieving restaurant with ID {RestaurantId}. Message: {Message}", RestaurantId, e.Message);
            return null;
        }
    }

    public async Task<bool> Create(Restaurant restaurant)
    {
        try
        {
            _logger.LogInformation("[RestaurantRepository] Attempting to create a new restaurant: {@Restaurant}.", restaurant);
            _db.Restaurants.Add(restaurant);
            await _db.SaveChangesAsync();
            _logger.LogInformation("[RestaurantRepository] Successfully created restaurant with ID {RestaurantId}.", restaurant.RestaurantId);
            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[RestaurantRepository] Error occurred while creating a new restaurant. Message: {Message}", e.Message);
            Console.WriteLine("Exception: " + e);
            return false;
        }
    }
    
    public async Task<bool> Update(Restaurant restaurant)
    {
        try
        {
        _logger.LogInformation("[RestaurantRepository] Attempting to update restaurant with ID {RestaurantId}.", restaurant.RestaurantId);
        _db.Restaurants.Update(restaurant);
        await _db.SaveChangesAsync();
        _logger.LogInformation("[RestaurantRepository] Successfully updated restaurant with ID {RestaurantId}.", restaurant.RestaurantId);
        return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "[RestaurantRepository] Error occurred while updating restaurant with ID {RestaurantId}. Message: {Message}", restaurant.RestaurantId, e.Message);
            return false;
        }
    
    }

    public async Task<bool> Delete(int restaurantId)
    {
        try
        {
           _logger.LogInformation("[RestaurantRepository] Attempting to delete restaurant with ID {RestaurantId}.", restaurantId); 
        // Find the restaurant including its reviews (if cascade delete is set up, reviews will be deleted automatically)
            var restaurant = await _db.Restaurants
                                    .Include(r => r.Reviews) 
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
        try
        {
        _logger.LogInformation("[RestaurantRepository] Retrieving restaurant with ID {RestaurantId}, including reviews and users.", RestaurantId);
        var restaurant = _db.Restaurants
            .Include(r => r.Reviews)
                .ThenInclude(review => review.User) // Include User data
            .FirstOrDefault(r => r.RestaurantId == RestaurantId); 
            
            if (restaurant == null)
            {
                _logger.LogWarning("[RestaurantRepository] No restaurant found with ID {RestaurantId}.", RestaurantId);
                throw new NullReferenceException($"No restaurant found with ID {RestaurantId}");
            }
            _logger.LogInformation("[RestaurantRepository] Successfully retrieved restaurant with ID {RestaurantId}, including reviews and users.", RestaurantId);
            return restaurant;
        }
        catch(Exception e)
        {
             _logger.LogError(e, "[RestaurantRepository] Error occurred while retrieving restaurant with ID {RestaurantId}. Message: {Message}", RestaurantId, e.Message);
            throw; // Rethrow the exception for higher-level handling
        }

    }

    public Restaurant GetItemAndReviewsById(int RestaurantId)
    {
        try
        {
            _logger.LogInformation("[RestaurantRepository] Retrieving restaurant with ID {RestaurantId}, including reviews and user data.", RestaurantId);

            var restaurant = _db.Restaurants
                .Include(r => r.Reviews)
                    .ThenInclude(review => review.User) // Include User data
                .FirstOrDefault(r => r.RestaurantId == RestaurantId); 
            if(restaurant == null)
            {
                _logger.LogWarning("[RestaurantRepository] No restaurant found with ID {RestaurantId}.", RestaurantId);
                throw new NullReferenceException($"No restaurant found with ID {RestaurantId}");
            }
            _logger.LogInformation("[RestaurantRepository] Successfully retrieved restaurant with ID {RestaurantId}, including reviews and user data.", RestaurantId);
            return restaurant;
        }
        catch(NullReferenceException e)
        {
            _logger.LogWarning(e, "[RestaurantRepository] NullReferenceException when retrieving restaurant with ID {RestaurantId}.", RestaurantId);
            throw; // Rethrow 
        }

    }
}