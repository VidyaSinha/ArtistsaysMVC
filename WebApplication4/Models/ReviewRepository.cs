using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using WebApplication4.Models;
using System;

namespace WebApplication4.Repositories
{
    public class ReviewRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;

        public List<Place> GetAllPlaces()
        {
            var places = new List<Place>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Places", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    places.Add(new Place
                    {
                        PlaceId = (int)reader["PlaceId"],
                        PlaceName = reader["PlaceName"].ToString(),
                        Location = reader["Location"].ToString(),
                        Description = reader["Description"].ToString()
                    });
                }
            }
            return places;
        }

        public List<Review> GetAllReviews()
        {
            var reviews = new List<Review>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT r.*, p.PlaceName FROM Reviews r JOIN Places p ON r.PlaceId = p.PlaceId", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reviews.Add(new Review
                    {
                        ReviewId = (int)reader["ReviewId"],
                        PlaceId = (int)reader["PlaceId"],
                        ReviewerName = reader["ReviewerName"].ToString(),
                        ReviewText = reader["ReviewText"].ToString(),
                        Rating = (int)reader["Rating"],
                        ReviewDate = (DateTime)reader["ReviewDate"],
                        PlaceName = reader["PlaceName"].ToString()
                    });
                }
            }
            return reviews;
        }

        public void AddReview(Review review)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "INSERT INTO Reviews (PlaceId, ReviewerName, ReviewText, Rating) VALUES (@PlaceId, @ReviewerName, @ReviewText, @Rating)", connection);
                command.Parameters.AddWithValue("@PlaceId", review.PlaceId);
                command.Parameters.AddWithValue("@ReviewerName", review.ReviewerName);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);
                command.ExecuteNonQuery();
            }
        }
        public Review GetReviewById(int reviewId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM Reviews WHERE ReviewId = @ReviewId", connection);
                command.Parameters.AddWithValue("@ReviewId", reviewId);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Review
                    {
                        ReviewId = (int)reader["ReviewId"],
                        PlaceId = (int)reader["PlaceId"],
                        ReviewerName = reader["ReviewerName"].ToString(),
                        ReviewText = reader["ReviewText"].ToString(),
                        Rating = (int)reader["Rating"],
                        ReviewDate = (DateTime)reader["ReviewDate"]
                    };
                }
            }
            return null;
        }

        public void UpdateReview(Review review)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "UPDATE Reviews SET ReviewerName = @ReviewerName, ReviewText = @ReviewText, Rating = @Rating, PlaceId = @PlaceId WHERE ReviewId = @ReviewId",
                    connection);
                command.Parameters.AddWithValue("@ReviewerName", review.ReviewerName);
                command.Parameters.AddWithValue("@ReviewText", review.ReviewText);
                command.Parameters.AddWithValue("@Rating", review.Rating);
                command.Parameters.AddWithValue("@PlaceId", review.PlaceId);
                command.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteReview(int reviewId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("DELETE FROM Reviews WHERE ReviewId = @ReviewId", connection);
                command.Parameters.AddWithValue("@ReviewId", reviewId);
                command.ExecuteNonQuery();
            }
        }

        public List<Review> GetReviewsByPlaceId(int placeId)
        {
            var reviews = new List<Review>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(
                    "SELECT r.*, p.PlaceName FROM Reviews r JOIN Places p ON r.PlaceId = p.PlaceId WHERE r.PlaceId = @PlaceId", connection);
                command.Parameters.AddWithValue("@PlaceId", placeId);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    reviews.Add(new Review
                    {
                        ReviewId = (int)reader["ReviewId"],
                        PlaceId = (int)reader["PlaceId"],
                        ReviewerName = reader["ReviewerName"].ToString(),
                        ReviewText = reader["ReviewText"].ToString(),
                        Rating = (int)reader["Rating"],
                        ReviewDate = (DateTime)reader["ReviewDate"],
                        PlaceName = reader["PlaceName"].ToString()
                    });
                }
            }
            return reviews;
        }


    }

}