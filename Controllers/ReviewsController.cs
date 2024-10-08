// Not in use yetm feel free to remove -nate 
using Microsoft.AspNetCore.Mvc;
using MapYourMeal.Models;

public class ReviewsController : Controller
{
    private readonly AppDbContext context;

    public ReviewsController(AppDbContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public IActionResult Create(Review review)
    {
        if (ModelState.IsValid)
        {
            // Add user id and restaurant id
            // review.UserId = ...;
            // review.RestaurantId = ...;

            context.Reviews.Add(review);
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(review);
    }
}