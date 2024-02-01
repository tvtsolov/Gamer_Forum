using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using System.Threading;

namespace Forum_System.Services
{
    public interface IThreadService
    {
        List<Thread> GetAll();
        PaginatedList<Thread> FilterBy(ThreadQueryParameters parameters);
        List<Thread> FilterBy();
        Thread GetById(int id);
        Thread GetByTitle(string title);
        Thread Create(int userId, Thread thread);
        Thread Update(int userId, int threadId, string updatedContent, string updatedTitle);
        bool Delete(int loggedId, int id);
        Thread AddRating(int threadId, double rating, int userId);

        bool TryRemoveRating(int ratingId, int threadId);
    }
}
