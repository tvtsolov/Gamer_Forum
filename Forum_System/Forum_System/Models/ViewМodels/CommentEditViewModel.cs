using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.ViewМodels
{
    public class CommentEditViewModel
    {
        [Required]
        [MinLength(32, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(8192, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string Content { get; set; }


        public DateTime DateEdited {  get; set; } = DateTime.Now;
    }
}
