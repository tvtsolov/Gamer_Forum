using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.ViewМodels;

namespace Forum_System.Helpers
{
    public class ModelMapper
    {
        //User DTOs
        public User MapCreate(UserCreateDto dto)
        {
            return new User()
            {
                Username = dto.Username,
                Email = dto.Email,
                Password = dto.Password,
                FirstName = dto.FirstName,
                LastName = dto.LastName
            };
        }

        public User MapUpdate(UserUpdateDto dto)
        {
            return new User()
            {
                Email = dto.Email,
                Password = dto.Password,
                Username = dto.Username,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                PhoneNumber = dto.PhoneNumber
            };
        }

        public UserCreatedDto MapCreatedToDTO(User user)
        {
            return new UserCreatedDto()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }


        //Thread DTOs
        public Thread MapCreate(ThreadCreateDto dto)
        {
            return new Thread()
            {
                Title = dto.Title,
                Content = dto.Content
            };
        }

        public ThreadFilteredDto MapFiltered(Thread thread)
        {
            return new ThreadFilteredDto()
            {
                Title = thread.Title,
                Content = thread.Content,
                Date = thread.Date,
                Author = thread.Author.Username,
                AvgRating = thread.Ratings?.Any() == true ? thread.Ratings.Average(r => r.Value) : 0.0,
                CommentsCount = thread.Comments?.Any() == true ? thread.Comments.Count() : 0,
            };
        }

        public ThreadCreatedResponseDto MapCreatedToDTO(Thread thread)
        {
            return new ThreadCreatedResponseDto
            {
                AuthorId = thread.AuthorId,
                Title = thread.Title,
                Content = thread.Content
            };
        }

        public ThreadUpdatedDto MapUpdate(Thread thread)
        {
            return new ThreadUpdatedDto()
            {
                Content = thread.Content,
                Title = thread.Title
            };
        }

        //Comment DTOs
        public Comment MapCreate(CommentCreateDto dto, User author)
        {
            return new Comment()
            {
                CreationDate = DateTime.Now,
                AuthorId = author.Id,
                Author = author,
                ThreadId = dto.ThreadId,
                Content = dto.Content
            };
        }

        public Comment MapUpdate(CommentUpdateDto dto)
        {
            return new Comment()
            {
                Content = dto.Content
            };
        }

        public ThreadRatedResponseDto MapRatedThread(Thread thread, int rating)
        {
            return new ThreadRatedResponseDto
            {
                ThreadId = thread.Id,
                NewRating = rating
            };
        }

        //MVC
        public User MapCreate(RegisterViewModel registerViewModel)
        {
            return new User
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                Email = registerViewModel.Email,
                Username = registerViewModel.Username,
                Password = registerViewModel.Password
            };
        }

        public ThreadDetailsViewModel MapThreadWithLoginID(Thread thread)
        {
            return new ThreadDetailsViewModel
            {
                Id = thread.Id,
                Title = thread.Title,
                Content = thread.Content,
                Date = thread.Date,
                Author = thread.Author,
                AuthorId=thread.AuthorId,
                Comments = thread.Comments,
                Ratings = thread.Ratings,
                IsEdited = thread.IsEdited,
                EditDate = thread.EditDate
            };
        }
    }
}
