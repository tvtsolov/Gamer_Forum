using Forum_System.Models.QueryParameters;
using Forum_System.Models;
using Forum_System.Models.DTOs;

namespace Forum_System.Services
{
    public interface ICommentService
    {
        List<Comment> GetAll();
        PaginatedList<Comment> FilterBy(CommentQueryParameters filterParameters);
        Comment GetById(int id);
        Comment Create(int loggedId, CommentCreateDto dto);
        Comment Update(int loggedId, int id, CommentUpdateDto dto);     //API
        Comment Update(int id, Comment comment);                        //MVC
        bool Delete(int loggedId, int id);

        bool Delete(int id);
        void CheckUserAuthorization(User loggedUser, Comment comment);
    }
}
