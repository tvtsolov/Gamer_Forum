using Forum_System.Models.Contracts;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Forum_System.Models
{
    public class Thread : IThread
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

        public User Author { get; set; }
        public int AuthorId {  get; set; } //navigation property
        public List<Comment> Comments { get; set; } //navigation property
        public DateTime Date { get; set; } = DateTime.Now;
        public List<Rating> Ratings { get; set; } //navigation property
		public List<Tag> Tags { get; set; }

		public bool Equals(Thread other)
        {
            if (other is null)
            {
                return false;
            }
            return Id == other.Id
                && Title == other.Title
                && Content == Content
                && Author == other.Author
                && AuthorId == other.AuthorId
                && Comments == other.Comments
                && Date == other.Date;
        }

        public bool IsEdited { get; set; }
        public DateTime EditDate { get; set; }

    }
}


