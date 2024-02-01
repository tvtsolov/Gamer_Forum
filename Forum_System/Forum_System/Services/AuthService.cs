using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Repositories.Contracts;
using Forum_System.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Forum_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IConfiguration config;

        public AuthService(IUserRepository userRepository, IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.config = config;
            this.httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(LoginRequest loginRequest)
        {
            var user = userRepository.GetByName(loginRequest.Username);

            List<Claim> claims = new List<Claim>
            {
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserID", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (user.IsAdmin)
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            else
            {
                claims.Add(new Claim(ClaimTypes.Role, "User"));
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config.GetSection("Jwt:Key").Value!) ); 

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                                                                    //HmacSha512Signature

            var Sectoken = new JwtSecurityToken(
                  claims: claims,
                  expires: DateTime.Now.AddMinutes(120),
                  signingCredentials: credentials
              );

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return token;
        }

        public User TryGetUser(string username)
        {
            return userRepository.GetByName(username)
                ?? throw new EntityNotFoundException($"User with username \"{username}\" was not found");
        }

        public void ValidateCredentials(LoginRequest loginRequest)
        {
            User user = userRepository.GetByName(loginRequest.Username);

            string encodedPassword = EncodePassword(loginRequest.Password);

            if (user.Password != loginRequest.Password)
            {
                throw new InvalidCredentialsException("Wrong credentials!");
            }
        }

        public User TryUserCredentials(string username, string password)
        {
            try
            {
                User user = userRepository.GetByName(username);

                password = EncodePassword(password);

                if (user.Password != password)
                {
                    throw new InvalidCredentialsException("Wrong credentials!");
                }

                return user;
            }
            catch (EntityNotFoundException)
            {
                throw new InvalidCredentialsException("Wrong credentials!");
            }


        }

        public int GetLoggedUserId()
        {
            int result = -1;

            if (httpContextAccessor.HttpContext is not null)
            {
                result = int.Parse(httpContextAccessor.HttpContext.User.FindFirstValue("UserID"));
            }

            return result;
        }

        public void CheckAdminAuthorization(User user)
        {
            if (!user.IsAdmin)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action!");
            }
        }

        public void CheckUserAuthorization(User loggedUser, User targetUser)
        {
            if (!loggedUser.IsAdmin && loggedUser.Id != targetUser.Id)
            {
                throw new UnauthorizedAccessException("You are not authorized to perform this action!");
            }
        }

        public string EncodePassword(string password)
        {
            var encodedPassword = Encoding.UTF8.GetBytes(password);

            return Convert.ToBase64String(encodedPassword);
        }
    }
}
