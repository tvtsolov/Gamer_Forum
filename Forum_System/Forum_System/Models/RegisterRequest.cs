using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models
{
    public class RegisterRequest
    {
        [Required]
        [MinLength(4, ErrorMessage = "{0} must be at least {1} charagters long")]
        [MaxLength(15, ErrorMessage = "{0} cannot be more then {1} charagters long")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}