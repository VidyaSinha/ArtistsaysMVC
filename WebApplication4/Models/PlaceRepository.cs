using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace WebApplication4.Models
{
    public class PlaceRepository
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;

        public List<Place> GetAllPlaces()
        {
            List<Place> places = new List<Place>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Places"; // Fetch all places
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    places.Add(new Place
                    {
                        //PlaceId = (int)reader["PlaceId"],
                        PlaceName = reader["PlaceName"].ToString(),
                        Location = reader["Location"] != DBNull.Value ? reader["Location"].ToString() : null,
                        Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null
                    });
                }

                connection.Close();
            }

            return places;
        }
    }
}
