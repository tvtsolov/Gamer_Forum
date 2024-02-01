using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;

namespace Forum_System.Repositories.Contracts
{
    public interface IUserRepository
    {
        List<User> GetAll();
        PaginatedList<User> FilterBy(UserQueryParameters parameters);
        //IList<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User GetByName(string username);
        User Create(User user);
        User Update(int id, User updatedData);
        User Promote(int id);
        User Demote(int id);
        User Block(int id);
        User Unblock(int id);
        bool Delete(int id);
        bool UserExists(string username);
        bool CheckDuplicateName(string username);
        bool CheckDuplicateEmail(string email);
    }
}
