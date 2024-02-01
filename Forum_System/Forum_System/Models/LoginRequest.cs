using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
