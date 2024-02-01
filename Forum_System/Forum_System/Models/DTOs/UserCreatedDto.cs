using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.DTOs
{
    public class UserCreatedDto
    {
        public int Id { get; set; } // maybe remove

        public string Username { get; set; }

        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
