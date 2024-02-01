using System.ComponentModel.DataAnnotations;

namespace Forum_System.Models.Contracts
{
    public interface IComment
    {
        int Id { get; set; }
        string Content { get; set; }
        IUser Author { get; set; }
        int AuthorId { get; set; }
        IThread Thread { get; set; }
        int ThreadId { get; set; }
        DateTime CreationDate { get; set; }
        int Rating { get; set; }
    }
}
