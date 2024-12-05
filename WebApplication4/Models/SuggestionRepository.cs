using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication4.Models
{
    public class SuggestionRepository
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;

        public void AddSuggestion(string name, string email, string suggestionText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Suggestions (Name, Email, SuggestionText) VALUES (@Name, @Email, @SuggestionText)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@SuggestionText", suggestionText);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void UpdateSuggestion(int id, string name, string email, string suggestionText)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Suggestions SET Name = @Name, Email = @Email, SuggestionText = @SuggestionText WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@SuggestionText", suggestionText);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public void DeleteSuggestion(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Suggestions WHERE Id = @Id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public List<SuggestionModel> GetSuggestions()
        {
            List<SuggestionModel> suggestions = new List<SuggestionModel>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Suggestions";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    suggestions.Add(new SuggestionModel
                    {
                        Id = (int)reader["Id"],
                        Name = reader["Name"].ToString(),
                        Email = reader["Email"].ToString(),
                        SuggestionText = reader["SuggestionText"].ToString()
                    });
                }
            }

            return suggestions;
        }
    }
}
