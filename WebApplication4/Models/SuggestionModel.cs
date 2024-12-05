using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models
{
    public class SuggestionModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SuggestionText { get; set; }
    }
}
