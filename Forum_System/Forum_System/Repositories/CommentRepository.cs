using Forum_System.Data;
using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Models.QueryParameters;
using Forum_System.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace Forum_System.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationContext context;

        public CommentRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Comment> GetAll()
        {
            return context.Comments.ToList();
        }

        public Comment Create(Comment comment)
        {
            context.Comments.Add(comment);
            context.SaveChanges();

            return comment;
        }

        public bool Delete(int id)
        {
            Comment comment = GetById(id);
            context.Comments.Remove(comment);

            return context.SaveChanges() > 0;
        }

        public Comment GetById(int id)
        {
            var comment = GetComments().FirstOrDefault(c => c.Id == id);

            return comment ?? throw new EntityNotFoundException($"Comment not found!");
        }

        public Comment Update(int id, Comment comment)
        {
            var commentToUpdate = GetById(id);
            if (commentToUpdate != null)
            {
                commentToUpdate.Content = comment.Content;
                commentToUpdate.Edited = true;
                commentToUpdate.EditDate = DateTime.Now;

                context.SaveChanges();
                return commentToUpdate;
            }
            else
            {
                throw new EntityNotFoundException($"No comment with id {id} exists");
            }

        }

        public PaginatedList<Comment> FilterBy(CommentQueryParameters parameters)
        {
            IQueryable<Comment> result = GetComments();

            result = FilterByAuthor(result, parameters.Author);
            result = FilterByFirstName(result, parameters.FirstName);
            result = FilterByLastName(result, parameters.LastName);
            result = FilterByContent(result, parameters.Content);
            result = FilterByThreadTitle(result, parameters.ThreadTitle);
            result = SortBy(result, parameters.SortBy);
            result = OrderBy(result, parameters.SortOrder);


            int totalPages = (int)Math.Ceiling(((double)result.Count()) / parameters.PageSize);

            result = result.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);

            return new PaginatedList<Comment>(result.ToList(), totalPages, parameters.PageNumber);
        }

        public IQueryable<Comment> SortBy(IQueryable<Comment> comments, string sortByCriteria)
        {
            {
                switch (sortByCriteria)
                {
                    case "date":
                        return comments.OrderBy(c => c.CreationDate);
                    case "content":
                        return comments.OrderBy(c => c.Content);
                    default:
                        return comments;
                }
            }
        }

        private static IQueryable<Comment> OrderBy(IQueryable<Comment> comments, string sortOrder)
        {
            return (sortOrder == "desc") ? comments.Reverse() : comments;
        }

        private static IQueryable<Comment> FilterByAuthor(IQueryable<Comment> comments, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                return comments.Where(c => c.Author.Username.ToLower().Contains(username));
            }
            else return comments;
        }

        private static IQueryable<Comment> FilterByContent(IQueryable<Comment> comments, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.ToLower();
                return comments.Where(c => c.Content.ToLower().Contains(content));
            }
            else return comments;
        }

        private static IQueryable<Comment> FilterByFirstName(IQueryable<Comment> comments, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                return comments.Where(c => c.Author.FirstName.ToLower().Contains(name));
            }
            else return comments;
        }

        private static IQueryable<Comment> FilterByLastName(IQueryable<Comment> comments, string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                name = name.ToLower();
                return comments.Where(c => c.Author.LastName.ToLower().Contains(name));
            }
            else return comments;
        }

        private static IQueryable<Comment> FilterByThreadTitle(IQueryable<Comment> comments, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = title.ToLower();
                return comments.Where(c => c.Thread.Title.ToLower().Contains(title));
            }
            else return comments;
        }

        private IQueryable<Comment> GetComments()
        {
            return context.Comments
                    .Include(c => c.Author)
                    .AsSplitQuery();
        }


    }
}
