using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Models.DTOs;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Forum_System.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly ModelMapper mapper;

        public AuthApiController(IAuthService authService, IUserService userService, ModelMapper mapper)
        {
            this.authService = authService;
            this.userService = userService;
            this.mapper = mapper;
        }

        [HttpPost("login")]

        public IActionResult Login([FromBody] LoginRequest loginCredentials)
        {
            try
            {
                var token = authService.GenerateToken(loginCredentials);

                return Ok(token);
            }
            catch (InvalidCredentialsException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException)
            {
                return Unauthorized("Wrong credentials!");
            }
            catch (InvalidUserInputException)
            {
                return Unauthorized("Wrong credentials!");
            }

        }


        [HttpPost("register")]
        public IActionResult Register([FromBody] UserCreateDto userDto)

        {     
            try
            {

                User createdUser = userService.Create(mapper.MapCreate(userDto));

                UserCreatedDto createdUserDto = new UserCreatedDto();
                createdUserDto = mapper.MapCreatedToDTO(createdUser);

                return StatusCode(StatusCodes.Status201Created, userDto);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return Conflict(e.Message);
            }
        }

        /*
        //Test Method
        [HttpGet]
        public IActionResult GetId()
        {
            var test = authService.GetLoggedUserId();

            return Ok(test);
        }
        */
    }
}
