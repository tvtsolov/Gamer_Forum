using Forum_System.Data;
using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Forum_System.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext context;

        public UserRepository(ApplicationContext context)
        {
            this.context = context;
        }

        public User Create(User user)
        {
           // CheckDuplicateName(user.Username);  //DuplicateEntityException
           // CheckDuplicateEmail(user.Email);    //DuplicateEntityException

            context.Users.Add(user);
            context.SaveChanges();

            return user;
        }

        public bool Delete(int id)
        {
            User user = GetById(id);
            context.Users.Remove(user);

            return context.SaveChanges() > 0;
        }

        public List<User> GetAll()  // not used at all
        {
            return context.Users.ToList();
        }

        public PaginatedList<User> FilterBy(UserQueryParameters parameters)
        {
            IQueryable<User> result = GetUsers();

            result = FilterByUsername(result, parameters.Username);
            result = FilterByFirstName(result, parameters.FirstName);
            result = FilterByLastName(result, parameters.LastName);
            result = FilterByEmail(result, parameters.Email);
            result = FilterByRole(result, parameters.Role);
            result = SortBy(result, parameters.SortBy);
            result = OrderBy(result, parameters.SortOrder);


            int totalPages = (int)Math.Ceiling(((double)result.Count()) / parameters.PageSize);

            result = result.Skip(parameters.PageSize * (parameters.PageNumber - 1)).Take(parameters.PageSize);

            return new PaginatedList<User>(result.ToList(), totalPages, parameters.PageNumber);
        }

        private static IQueryable<User> OrderBy(IQueryable<User> users, string sortOrder)
        {
            return (sortOrder == "desc") ? users.Reverse() : users;
        }

        public IQueryable<User> SortBy(IQueryable<User> users, string sortByCriteria)
        {
            {
                switch (sortByCriteria)
                {
                    case "id":
                        return users.OrderBy(u => u.Id);
                    case "username":
                        return users.OrderByDescending(u => u.Username);
                    case "firstname":
                        return users.OrderByDescending(u => u.FirstName);
                    case "lastname":
                        return users.OrderByDescending(u => u.LastName);
                    case "email":
                        return users.OrderByDescending(u => u.Email);
                    case "role":
                        return users.OrderBy(u => u.IsAdmin).ThenBy(u => u.IsBlocked);
                    case "comments":
                        return users.OrderByDescending(u => u.Comments.Count());
                    case "threads":
                        return users.OrderByDescending(u => u.Threads.Count());
                    default:
                        return users;
                }
            }
        }

        private static IQueryable<User> FilterByRole(IQueryable<User> users, string role)
        {
            if (!string.IsNullOrEmpty(role))
            {
                role = role.ToLower();

                if (role == "admin")
                {
                    return users.Where(u => u.IsAdmin == true);
                }
                else if (role == "blocked")
                {
                    return users.Where(u => u.IsBlocked == true);
                }
                else if (role == "user")
                {
                    return users.Where(u => u.IsBlocked == false && u.IsAdmin == false);
                }
                else return users;

            }
            else return users;
        }


        private static IQueryable<User> FilterByUsername(IQueryable<User> users, string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                username = username.ToLower();
                return users.Where(u => u.Username.ToLower().Contains(username));
            }
            else return users;
        }

        private static IQueryable<User> FilterByFirstName(IQueryable<User> users, string firstName)
        {
            if (!string.IsNullOrEmpty(firstName))
            {
                firstName = firstName.ToLower();
                return users.Where(u => u.FirstName.ToLower().Contains(firstName));
            }
            else return users;
        }

        private static IQueryable<User> FilterByLastName(IQueryable<User> users, string lastName)
        {
            if (!string.IsNullOrEmpty(lastName))
            {
                lastName = lastName.ToLower();
                return users.Where(u => u.LastName.ToLower().Contains(lastName));
            }
            else return users;
        }

        private static IQueryable<User> FilterByEmail(IQueryable<User> users, string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                email = email.ToLower();
                return users.Where(u => u.Email.ToLower().Contains(email));
            }
            else return users;
        }

        public User GetById(int id)
        {
            User user = GetUsers().FirstOrDefault(u => u.Id == id);

            return user ?? throw new EntityNotFoundException($"User not found!");
        }

        public User GetByName(string username)
        {
            User user = context.Users.FirstOrDefault(u => u.Username == username);

            return user ?? throw new EntityNotFoundException($"User not found!");
        }

        public bool UserExists(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }

        public User Promote(int id)
        {
            var user = GetById(id);

            if (user.IsAdmin == true)
            {
                throw new InvalidUserInputException($"User {user.Username} is already an admin!");
            }

            user.IsAdmin = true;
            context.SaveChanges();
            return user;
        }

        public User Demote(int id)
        {
            var user = GetById(id);

            if (user.IsAdmin == false)
            {
                throw new InvalidUserInputException($"User {user.Username} is already a regular user!");
            }

            user.IsAdmin = false;
            user.PhoneNumber = null;

            context.SaveChanges();
            return user;
        }

        public User Block(int id)
        {
            var user = GetById(id);

            if (user.IsBlocked == true)
            {
                throw new InvalidUserInputException($"User {user.Username} is already blocked!");
            }

            user.IsBlocked = true;
            context.SaveChanges();
            return user;
        }

        public User Unblock(int id)
        {
            var user = GetById(id);

            if (user.IsBlocked == false)
            {
                throw new InvalidUserInputException($"User {user.Username} is already unblocked!");
            }

            user.IsBlocked = false;
            context.SaveChanges();
            return user;
        }

        public User Update(int id, User updatedData)
        {
            var userToUpdate = GetById(id);

            userToUpdate.FirstName = updatedData.FirstName ?? userToUpdate.FirstName;
            userToUpdate.LastName = updatedData.LastName ?? userToUpdate.LastName;
            userToUpdate.Email = updatedData.Email ?? userToUpdate.Email;

            if (updatedData.Username != null)
            {
                if (!userToUpdate.IsAdmin)
                {
                    throw new UnauthorizedAccessException("Only admin users can change their username!");
                }

                userToUpdate.Username = updatedData.Username;
            }

            userToUpdate.Password = updatedData.Password ?? userToUpdate.Password;

            if (updatedData.PhoneNumber != null)
            {
                if (!userToUpdate.IsAdmin)
                {
                    throw new UnauthorizedAccessException("Only administrators can add a phone number!");
                }

                userToUpdate.PhoneNumber = updatedData.PhoneNumber;
            }

            context.Update(userToUpdate);
            context.SaveChanges();

            return userToUpdate;
        }

        public bool CheckDuplicateName(string username)
        {
            return context.Users.Any(u => u.Username == username);
        }

        public bool CheckDuplicateEmail(string email)
        {
            return context.Users.Any(u => u.Email == email);
        }

        private IQueryable<User> GetUsers()
        {
            return context.Users
                    .Include(u => u.Threads)
                    .Include(u => u.Comments)
                    .AsSplitQuery();
        }

    }
}
