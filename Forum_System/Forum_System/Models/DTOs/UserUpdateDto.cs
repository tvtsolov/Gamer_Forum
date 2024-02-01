using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.DTOs
{
    public class UserUpdateDto
    {
        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string? FirstName { get; set; }

        [MinLength(4, ErrorMessage = "The {0} must be at least {1} characters long.")]
        [MaxLength(32, ErrorMessage = "The {0} must be no more than {1} characters long.")]
        public string? LastName { get; set; }

        public string? Username { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string? Email { get; set; } 
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
