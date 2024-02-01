using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.DTOs
{
    public class ThreadRatedResponseDto
    {
        [Required]
        public int ThreadId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "The {0} must be a value between {1} and {2}")]
        public int NewRating { get; set; }
    }
}
