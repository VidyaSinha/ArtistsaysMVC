using System;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication4.Repositories
{
    public class SubmitRepository
    {
        private readonly string connectionString;

        public SubmitRepository()
        {

            connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"]?.ConnectionString;

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Connection string 'MyDbContext' is not configured in Web.config.");
            }
        }

        public void AddContact(string name, string email, string message)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO Contacts (Name, Email, Message) VALUES (@Name, @Email, @Message)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Use parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@Name", name);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Message", message);

                        connection.Open(); // Open the connection
                        command.ExecuteNonQuery(); // Execute the insert command
                    }
                }
            }
            catch (SqlException ex)
            {
                // Handle or log the exception as needed
                throw new Exception("An error occurred while adding contact: " + ex.Message);
            }
        }
    }
}
