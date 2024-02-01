using Forum_System.Models.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string LastName { get; set; }



        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress (ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } //must be unique, can be set in OnModelCreating with .IsUnique();

        [Required]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsBlocked { get; set; }

        public List<Thread> Threads { get; set; } // nav property
        public List<Comment> Comments { get; set; } // nav property
        public List<Rating> Ratings {  get; set; } // nav property 
    }
}


