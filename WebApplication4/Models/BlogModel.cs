using System;

namespace WebApplication4.Models
{
    public class BlogPost
    {
        public int Id { get; set; }         // Auto-incrementing primary key
        public string Title { get; set; }   // Title of the blog post
        public DateTime PostDate { get; set; } // Date when the post is created
        public string Content { get; set; }  // Content of the blog post
    }
}
