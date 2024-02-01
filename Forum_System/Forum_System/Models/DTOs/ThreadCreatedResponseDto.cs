using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.DTOs
{
    public class ThreadCreatedResponseDto
    {
        public int AuthorId { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
