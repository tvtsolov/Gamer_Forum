using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;

namespace Forum_System.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository repository;
        private readonly IUserService userService;
        private readonly IAuthService authService;
        private readonly ModelMapper mapper;

        public CommentService(ICommentRepository repository, IUserService userService, IAuthService authService, ModelMapper mapper)
        {
            this.repository = repository;
            this.userService = userService;
            this.authService = authService;
            this.mapper = mapper;
        }

        public List<Comment> GetAll()
        {
            return repository.GetAll();
        }

        public Comment GetById(int id)
        {
            return repository.GetById(id);
        }

        public Comment Create(int loggedId, CommentCreateDto dto)
        {
            var loggedUser = userService.GetById(loggedId);
            userService.CheckUserBlockStatus(loggedUser);

            var comment = mapper.MapCreate(dto, loggedUser);

            return repository.Create(comment);
        }

        public bool Delete(int loggedId, int id)                                //API
        {
            var loggedUser = userService.GetById(loggedId);
            var commentToDelete = repository.GetById(id);

            CheckUserAuthorization(loggedUser, commentToDelete);

            return repository.Delete(id);
        }

        public bool Delete(int id)                                              //MVC
        {
            return repository.Delete(id);
        }


        public Comment Update(int loggedId, int id, CommentUpdateDto dto)       //API
        {
            var loggedUser = userService.GetById(loggedId);
            var commentToUpdate = repository.GetById(id);

            userService.CheckUserBlockStatus(loggedUser);
            CheckUserAuthorization(loggedUser, commentToUpdate);

            // Rewrite
            commentToUpdate.Content = dto.Content;

            return repository.Update(id, commentToUpdate);
        }

        public Comment Update(int id, Comment comment)                          // MVC
        {
            var commentToUpdate = repository.GetById(id);

            commentToUpdate.Content = comment.Content;

            return repository.Update(id, commentToUpdate);
        }

        public PaginatedList<Comment> FilterBy(CommentQueryParameters filterParameters)
        {
            return repository.FilterBy(filterParameters);
        }

        // move to authservice?
        public void CheckUserAuthorization(User loggedUser, Comment comment)
        {
            if (!loggedUser.IsAdmin && loggedUser.Id != comment.AuthorId)
            {
                throw new UnauthorizedAccessException("You are not the author of this post!");
            }
        }
    }
}
