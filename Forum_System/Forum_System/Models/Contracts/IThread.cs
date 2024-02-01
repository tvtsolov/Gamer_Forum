using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.Contracts
{
    public interface IThread
    {
        int Id { get; set; }
        string Title { get; set; }
        string Content { get; set; }
        User Author { get; set; }
        int AuthorId { get; set; } 
        List<Comment> Comments { get; set; } 
        DateTime Date { get; set; }
    }
}
