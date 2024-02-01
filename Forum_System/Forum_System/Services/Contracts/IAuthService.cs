using Forum_System.Models;
using Forum_System.Models.DTOs;

namespace Forum_System.Services.Contracts
{
    public interface IAuthService
    {
        string GenerateToken(LoginRequest loginRequest);
        void ValidateCredentials(LoginRequest loginCredentials);
        int GetLoggedUserId();
        User TryUserCredentials(string username, string password);
        void CheckAdminAuthorization(User user);
        void CheckUserAuthorization(User loggedUser, User targetUser);
        public User TryGetUser(string username);

        //void CheckAnonymousUser();
        string EncodePassword(string password);
    }
}
