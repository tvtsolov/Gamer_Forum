using Forum_System.Models;
using Forum_System.Models.QueryParameters;

namespace Forum_System.Repositories.Contracts
{
    public interface ICommentRepository
    {
        List<Comment> GetAll();
        PaginatedList<Comment> FilterBy(CommentQueryParameters parameters);  
        Comment GetById(int id);
        Comment Create(Comment comment);
        Comment Update(int id, Comment comment);
        bool Delete(int id);

    }
}
