using Microsoft.EntityFrameworkCore;
using MapYourMeal.Models;

namespace MapYourMeal.DAL;
public class RestaurantRepository : IRestaurantRepository
{
    private readonly ApplicationDbContext _db;

    public RestaurantRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Restaurant>> GetAll()
    {
        return await _db.Restaurants.ToListAsync();
    }

    public async Task<Restaurant?> GetItemById(int RestaurantId)
    {
        return await _db.Restaurants.FindAsync(RestaurantId); // Kontroller at id eksisterer i databasen
    }

    public async Task Create(Restaurant restaurant)
    {
        _db.Restaurants.Add(restaurant);
        await _db.SaveChangesAsync();
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
}