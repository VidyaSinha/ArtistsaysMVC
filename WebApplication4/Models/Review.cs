using System;

namespace WebApplication4.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int PlaceId { get; set; }
        public string ReviewerName { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }

        public string PlaceName { get; set; } // To display the place name
    }
}