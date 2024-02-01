using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;

namespace Forum_System.Repositories.Contracts
{
    public interface IThreadRepository
    {
        List<Thread> GetAll();
        PaginatedList<Thread> FilterBy(ThreadQueryParameters parameters);
        List<Thread> FilterBy();
        Thread GetById(int id);
        Thread GetByTitle(string title);
        Thread Create(Thread thread);
        Thread Update(int userId, int threadId, string updatedContent, string UpdatedTitle);
        bool Delete(int id);
        bool CheckDuplicateTitle(string title);
        Thread AddRating(int threadId, double rating, int userId);

        bool TryRemoveRating(int ratingId, int threadId);
    }
}
