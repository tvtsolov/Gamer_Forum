using Forum_System.Data;
using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
using System.Xml.Linq;

namespace Forum_System.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly ApplicationContext context;
        public ThreadRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public List<Thread> GetAll()
        {
            return context.Threads
                .Include(t => t.Comments)
                .Include(t => t.Author)
                .Include(t => t.Ratings)
                .ToList();
        }

        public PaginatedList<Thread> FilterBy(ThreadQueryParameters parameters)
        {

            IQueryable<Thread> result = GetThreads();

            result = FilterByTitle(result, parameters.Title);
            result = FilterByContent(result, parameters.Content);
            result = FilterByAuthorUsername(result, parameters.Author);
            result = SortBy(result, parameters.SortBy);
            result = FilterByMinRating(result, parameters.MinRating);
            result = FilterByMaxRating(result, parameters.MaxRating);
            result = OrderBy(result, parameters.SortOrder);

            //result = FilterByStartDate(result, parameters.StartDate);
            //result = FilterByEndDate(result, parameters.EndDate);

            int totalPages = (int)Math.Ceiling(((double)result.Count()) / parameters.PageSize); 
            
            result = result.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);

            return new PaginatedList<Thread>(result.ToList(), totalPages, parameters.PageNumber);

        }

        private static IQueryable<Thread> OrderBy(IQueryable<Thread> threads, string sortOrder)
        {
            return (sortOrder == "desc") ? threads.Reverse() : threads;
        }

        public List<Thread> FilterBy()
        {
            IQueryable<Thread> result = GetThreads();
            return result.ToList();
        }
        private static IQueryable<Thread> FilterByMinRating(IQueryable<Thread> threads, int? minRating)
        {
            if (minRating.HasValue)
                return threads.Where(t => t.Ratings.Average(r => r.Value) >= minRating);
            else
                return threads;
        }

        private static IQueryable<Thread> FilterByMaxRating(IQueryable<Thread> threads, int? maxRating)
        {
            if (maxRating.HasValue)
                return threads.Where(t => t.Ratings.Average(r => r.Value) <= maxRating);
            else 
                return threads;
        }

        private static IQueryable<Thread> FilterByTitle(IQueryable<Thread> threads, string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                title = title.ToLower();
                return threads.Where(thread => thread.Title.ToLower().Contains(title));
            }
            else
                return threads;
        }

        private static IQueryable<Thread> FilterByContent(IQueryable<Thread> threads, string content)
        {
            if (!string.IsNullOrEmpty(content))
            {
                content = content.ToLower();
                return threads.Where(thread => thread.Content.ToLower().Contains(content));
            }
            
            else
                return threads;
        }

        private static IQueryable<Thread> FilterByAuthorUsername(IQueryable<Thread> threads, string author)
        {
            if (!string.IsNullOrEmpty(author))
            {
                author = author.ToLower();
                return threads.Where(thread => thread.Author.Username.ToLower().Contains(author));
            }
            else
                return threads;
        }

        public IQueryable<Thread> SortBy(IQueryable<Thread> threads, string sortByCriteria)
        {
            {
                switch (sortByCriteria)
                {
                    case "title":
                        return threads.OrderByDescending(thread => thread.Title);
                    case "date":
                        return threads.OrderBy(thread => thread.Date);
                    case "ratings":
                        return threads.OrderBy(thread => thread.Ratings.Any() ? thread.Ratings.Average(r => r.Value) : 0);
                    case "comments":
                        return threads.OrderByDescending(thread => thread.Comments.Count);
                    default:
                        return threads;
                }
            }
        }

        public Thread GetById(int id)
        {
            Thread thread = GetThreads().FirstOrDefault(t => t.Id == id);

            return thread ?? throw new EntityNotFoundException($"Thread with ID {id} not found!");
        }

        public Thread GetByTitle(string title)
        {
            Thread thread = context.Threads.FirstOrDefault(t => t.Title == title);
            return thread ?? throw new EntityNotFoundException($"Thread with title \"{title}\" not found");
        }

        public Thread Create(Thread thread)
        {
            context.Threads.Add(thread);
            context.SaveChanges();

            return thread;
        }

        public bool Delete(int id)
        {
            var thread = GetById(id);
            context.Threads.Remove(thread);

            return context.SaveChanges() > 0;
        }

        public bool CheckDuplicateTitle(string title)
        {
            return context.Threads.Any(b => b.Title == title);
        }

        private IQueryable<Thread> GetThreads()
        {
            return context.Threads
                .Include(t => t.Author)
                .Include(t => t.Tags)
                .Include(t => t.Comments)
                    .ThenInclude(c => c.Author)
                .Include(t => t.Ratings)
                    .ThenInclude(r => r.User)
                .AsSplitQuery();
        }

        public Thread AddRating(int threadId, double rating, int userId)
        {
            var threadToRate = GetById(threadId);
            var newRating = new Rating
            {
                ThreadId = threadToRate.Id,
                UserId = userId,
                Value = rating
            };
            context.Ratings.Add(newRating);
            context.SaveChanges();

            return threadToRate;
        }

        public bool TryRemoveRating(int ratingId, int threadId)
        {
            var ratingToRemove = context.Ratings.FirstOrDefault(r => r.Id == ratingId);
            if(ratingToRemove != null) 
            {
                context.Ratings.Remove(ratingToRemove);
                return context.SaveChanges() > 0;
            }
            else
            {
                throw new EntityNotFoundException("Rating does not exist");
            }
        }


        public Thread Update(int userId, int threadId, string updatedContent, string updatedTitle)
        {
            var threadToUpdate = GetById(threadId);
            
            if(!IsAuthor(userId, threadToUpdate))
                throw new UnauthorizedOperationException("Only the author or admin can edit the thread");

            threadToUpdate.Content = updatedContent;
            threadToUpdate.Title = updatedTitle;
            threadToUpdate.IsEdited = true;
            threadToUpdate.EditDate = DateTime.Now;

            context.SaveChanges();
            return threadToUpdate;
        }

        private bool IsAuthor(int userId, Thread thread)
        {
            return thread.AuthorId == userId;
        }
    }
}
