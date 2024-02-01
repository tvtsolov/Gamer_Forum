using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Models.ViewМodels;

namespace Forum_System.Services
{
    public interface IUserService
    {
        IList<User> GetAll();
        PaginatedList<User> FilterBy(UserQueryParameters filterParameters);
        User GetById(int id);
        User GetByName(string username);
        User Create(User user);
        User Update(int loggedId, int idToUpdate, User user);
        User Promote(int loggedId, int idToPromote);
        User Demote(int loggedId, int id);
        User Block(int loggedId, int id);
        User Unblock(int loggedId, int id);
        bool Delete(int loggedId, int id);
        public bool UserExists(string username);
        void ChangeAvatar(string username, int loggedId, AvatarViewModel avatarVM);
        void RemoveAvatar(int id, int loggedId);
        void CheckUserBlockStatus(User loggedUser);
    }
}
