using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Forum_System.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthService authService;
        private readonly ModelMapper mapper;
        public UsersApiController(IUserService userService, IAuthService authService, ModelMapper mapper)
        {
            this.userService = userService;
            this.authService = authService;
            this.mapper = mapper;
        }

        [Authorize]
        [HttpGet("")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetAll([FromQuery] UserQueryParameters filterParameters)
        {
            try
            {
                var users = userService.FilterBy(filterParameters);
                return Ok(users);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetById(int id)
        {
            try
            {
                var user = userService.GetById(id);

                return Ok(user);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Update(int id, [FromBody] UserUpdateDto updateData)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var updatedUser = userService.Update(loggedId, id, mapper.MapUpdate(updateData));

                return Ok(updatedUser);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                bool userDeleted = userService.Delete(loggedId, id);

                return Ok(userDeleted);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("promote/{id}")] //must be admin
        [Authorize(Roles = "Admin")]
        public IActionResult Promote(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var promotedUser = userService.Promote(loggedId, id);

                return StatusCode(StatusCodes.Status200OK, promotedUser);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidUserInputException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("demote/{id}")] //must be admin
        [Authorize(Roles = "Admin")]
        public IActionResult Demote(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var demotedUser = userService.Demote(loggedId, id);

                return StatusCode(StatusCodes.Status200OK, demotedUser);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidUserInputException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("block/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Block(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var blockedUser = userService.Block(loggedId, id);

                return StatusCode(StatusCodes.Status200OK, blockedUser);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidUserInputException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut("unblock/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Unblock(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var unblockedUser = userService.Unblock(loggedId, id);

                return StatusCode(StatusCodes.Status200OK, unblockedUser);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (InvalidUserInputException e)
            {
                return Conflict(e.Message);
            }
        }

    }
}
