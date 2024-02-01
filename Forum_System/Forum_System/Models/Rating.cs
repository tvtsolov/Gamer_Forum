using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public int? ThreadId { get; set; }
        public Thread Thread { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }

        [Range(1, 5)]
        public double Value { get; set; }
    }
}
