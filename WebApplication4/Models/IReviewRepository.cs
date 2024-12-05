using System.Collections.Generic;

namespace WebApplication4.Models
{
    public interface IReviewRepository
    {
        void AddReview(Review review);
        List<Review> GetAllReviews();
        List<Place> GetAllPlaces();
    }
}
