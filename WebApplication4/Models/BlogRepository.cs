using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace WebApplication4.Models
{
    public class BlogPostRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MyDbContext"].ConnectionString;

        public List<BlogPost> GetAllPosts()
        {
            List<BlogPost> posts = new List<BlogPost>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM BlogPosts";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BlogPost post = new BlogPost
                        {
                            Id = (int)reader["Id"],
                            Title = reader["Title"].ToString(),
                            PostDate = (DateTime)reader["PostDate"],
                            Content = reader["Content"].ToString()
                        };
                        posts.Add(post);
                    }
                }
            }
            return posts;
        }

        public void AddPost(BlogPost post)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO BlogPosts (Title, PostDate, Content) VALUES (@Title, @PostDate, @Content)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Title", post.Title);
                command.Parameters.AddWithValue("@PostDate", post.PostDate);
                command.Parameters.AddWithValue("@Content", post.Content);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
