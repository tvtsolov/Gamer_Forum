using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Models.ViewМodels;
using Forum_System.Repositories;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Forum_System.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;
        private readonly IAuthService authService;
         
        public UserService(IUserRepository repository, IAuthService authService)
        {
            this.repository = repository;
            this.authService = authService;
        }

        public IList<User> GetAll()  //not used at all
        {
            return repository.GetAll();
        }

        public PaginatedList<User> FilterBy(UserQueryParameters filterParameters)
        {
            return repository.FilterBy(filterParameters);
        }

        public User GetById(int id)
        {
            return repository.GetById(id);
        }

        public User Create(User user)
        {
            if (repository.CheckDuplicateName(user.Username))
            {
                throw new DuplicateEntityException($"Username {user.Username} is already in use!");
            }
                
            if (repository.CheckDuplicateEmail(user.Email))
            {
                throw new DuplicateEntityException($"Email {user.Email} is already in use!");
            }
               
            user.Password = authService.EncodePassword(user.Password);

            return repository.Create(user);
        }

        public bool Delete(int loggedId, int id)
        {
            var loggedUser = repository.GetById(loggedId);
            authService.CheckAdminAuthorization(loggedUser);

            return repository.Delete(id);
        }

        public User GetByName(string username)
        {
            return repository.GetByName(username);
        }

        public bool UserExists(string username)
        {
            return repository.UserExists(username);
        }

        public User Promote(int loggedId, int idToPromote)
        {
            var loggedUser = repository.GetById(loggedId);
            var userToPromote = repository.GetById(idToPromote);

            authService.CheckAdminAuthorization(loggedUser);

            return repository.Promote(idToPromote);
        }

        public User Demote(int loggedId, int idToDemote)
        {
            var loggedUser = repository.GetById(loggedId);
            var userToDemote = repository.GetById(idToDemote);

            authService.CheckAdminAuthorization(loggedUser);

            return repository.Demote(idToDemote);
        }

        public User Block(int loggedId, int idToBlock)
        {
            var loggedUser = repository.GetById(loggedId);
            var userToBlock = repository.GetById(idToBlock);

            authService.CheckAdminAuthorization(loggedUser);

            return repository.Block(idToBlock);
        }

        public User Unblock(int loggedId, int idToUnblock)
        {
            var loggedUser = repository.GetById(loggedId);
            var userToUnblock = repository.GetById(idToUnblock);

            authService.CheckAdminAuthorization(loggedUser);

            return repository.Unblock(idToUnblock);
        }

        public User Update(int loggedId, int idToUpdate, User updateData)
        {
            var loggedUser = repository.GetById(loggedId);
            var userToUpdate = repository.GetById(idToUpdate);

            authService.CheckUserAuthorization(loggedUser, userToUpdate);

            return repository.Update(idToUpdate, updateData);
        }

        public void ChangeAvatar(string username, int loggedId, AvatarViewModel avatarVM)
        {
            avatarVM.FileName = $"{username}{DateTime.Now.Ticks}.jpg";

            string avatarDir = "wwwroot/images/avatars";
            bool hasAvatar = Directory.EnumerateFiles(avatarDir, $"{username}*").Any();

            if (hasAvatar)
            {
                var directory = new DirectoryInfo(@"wwwroot/images/avatars");
                var fileInfo = directory.GetFiles("*" + username + "*.*");
                string avatarName = fileInfo[0].Name;
                string oldPath = Path.Combine("wwwroot/images/avatars", avatarName);
                System.IO.File.Delete(oldPath);
            }

            string newPath = Path.Combine("wwwroot/images/avatars", avatarVM.FileName);

            using (var fileStream = new FileStream(newPath, FileMode.Create))
            {
                avatarVM.Picture.CopyTo(fileStream);
            }
        }

        public void RemoveAvatar(int id, int loggedId)
        {
            var currentUser = GetById(loggedId);

            if (loggedId == id || currentUser.IsAdmin)
            {
                var user = GetById(id);
                string username = user.Username;

                string avatarDir = "wwwroot/images/avatars";
                bool hasAvatar = Directory.EnumerateFiles(avatarDir, $"{username}*").Any();

                if (hasAvatar)
                {
                    var directory = new DirectoryInfo(@"wwwroot/images/avatars");
                    var fileInfo = directory.GetFiles("*" + username + "*.*");
                    string avatarName = fileInfo[0].Name;
                    string oldPath = Path.Combine("wwwroot/images/avatars", avatarName);
                    System.IO.File.Delete(oldPath);
                }
                else
                {
                    throw new EntityNotFoundException("File not found!");
                }
            }
            else
            {
                throw new UnauthorizedOperationException("You are unauthorized to make this change!");
            }

           
        }

        public void CheckUserBlockStatus(User loggedUser)
        {
            if (loggedUser.IsBlocked == true)
            {
                throw new UnauthorizedAccessException("You are currently blocked!");
            }
        }

    }
}
