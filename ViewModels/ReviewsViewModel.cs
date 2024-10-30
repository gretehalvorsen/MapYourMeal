using MapYourMeal.Models;


namespace MapYourMeal.ViewModels
{
    public class ReviewsViewModel
    {
        public IEnumerable<Review> Reviews { get;}
        public string? CurrentViewName { get; }
        public ReviewsViewModel(IEnumerable<Review> reviews, string? currentViewName)
        {
            Reviews = reviews;
            CurrentViewName = currentViewName;
        }
    }
}