using Forum_System.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models
{
    public class Comment //: IComment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(32, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }
        public User Author { get; set; }
        public int AuthorId { get; set; }
        public Thread Thread { get; set; }
        public int ThreadId { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public bool Edited { get; set; }
        public DateTime EditDate { get; set; }
    }
}
