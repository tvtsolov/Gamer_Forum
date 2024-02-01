using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.Contracts
{
    public interface IUser
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        public string Username { get; set; }
        string Email { get; set; }
        public string Password { get; set; }
        string PhoneNumber { get; set; }
        bool IsAdmin { get; set; }
        bool IsBlocked { get; set; }
        List<Thread> Threads { get; set; } // nav property
        List<Comment> Comments { get; set; } // nav property
    }
}
