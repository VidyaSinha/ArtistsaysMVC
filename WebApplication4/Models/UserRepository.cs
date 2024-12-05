using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication4.Repositories
{
    public class UserRepository
    {
        private readonly string connectionString = "Data Source=LAPTOP-SE92TMUF\\SQLEXPRESS;Initial Catalog=Demo;Integrated Security=True;";

        // Method to Register User
        public bool RegisterUser(string username, string password, string email)
        {
            string hashedPassword = HashPassword(password);  // Hash the password

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Users (Username, Password, Email) VALUES (@Username, @Password, @Email)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);
                    command.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Method to Log In User
        public bool LoginUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(1) FROM Users WHERE Username = @Username AND Password = @Password";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", hashedPassword);

                    int userCount = (int)command.ExecuteScalar();
                    return userCount > 0;
                }
            }
        }

        // Method to Hash Password
        private string HashPassword(string password)
        {
            /* using (SHA256 sha256Hash = SHA256.Create())
             {
                 byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                 StringBuilder builder = new StringBuilder();
                 foreach (byte b in bytes)
                 {
                     builder.Append(b.ToString("x2"));
                 }
                 return builder.ToString();
             }*/
            return password;
        }
    }
}