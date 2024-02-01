using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.ViewМodels
{
    public class ThreadDetailsViewModel
    {
        public int Id { get; set; }


        [Required]
        [MinLength(16, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(64, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Title { get; set; }

        [Required]
        [MinLength(32, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }

        public DateTime Date { get; set; }
        public int LoggedUserId { get; set; }

        public  User Author { get; set; }
        public int AuthorId { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Rating> Ratings { get; set; }

        public bool IsEdited { get; set; }
        public DateTime EditDate { get; set; }

    }
}
