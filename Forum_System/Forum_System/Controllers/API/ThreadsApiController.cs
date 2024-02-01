using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.Contracts;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Abstractions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Security.Claims;

namespace Forum_System.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/threads")]
    public class ThreadsApiController : ControllerBase
    {
        private readonly IThreadService threadService;
        private readonly ModelMapper modelMapper;
        private readonly IAuthService authService;
        public ThreadsApiController(IThreadService threadService, ModelMapper modelMapper, IAuthService authService)
        {
            this.threadService = threadService;
            this.modelMapper = modelMapper;
            this.authService = authService;
        }

        [HttpGet("")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetThreads([FromQuery] ThreadQueryParameters filterParameters)
        {
            List<ThreadFilteredDto> threads = threadService
                .FilterBy(filterParameters)
                .Select(thread => modelMapper.MapFiltered(thread))
                .ToList();

            return Ok(threads);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetById(int id)
        {
            try
            {
                var thread = threadService.GetById(id);

                return Ok(thread);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpPost("")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create([FromBody] ThreadCreateDto dto)
        {
            try
            {
                string username = User.FindFirstValue(ClaimTypes.Name);  //todo this can be done in the Create comment method
                string userIdStr = User.FindFirstValue("UserID");
                int userId = int.Parse(userIdStr);

                Thread thread = modelMapper.MapCreate(dto);

                Thread createdThread = threadService.Create(userId, thread);
                ThreadCreatedResponseDto createdThreadDto = modelMapper.MapCreatedToDTO(createdThread);

                return StatusCode(StatusCodes.Status201Created, createdThreadDto);

            }
            catch (UnauthorizedOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }


        }

        // Add catch for unauthorized user
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public IActionResult Update(int id, [FromBody] ThreadCreateDto dto)
        {
            try
            {
                if (dto.Content.Length < 32)
                {
                    throw new InvalidUserInputException("The content of the thread must be at least 32 characters");
                }
                if (dto.Title.Length <16  || dto.Title.Length >64)
                {
                    throw new InvalidUserInputException("The title of the thread must be between 16 and 64 characters");
                }


                string userIdStr = User.FindFirstValue("UserID");
                int userId = int.Parse(userIdStr);
                ThreadUpdatedDto updatedThread = modelMapper.MapUpdate(threadService.Update(userId, id, dto.Content, dto.Title));
                return Ok(updatedThread);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidUserInputException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Admin or author
        // Add catch for unauthorized user 
        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                bool threadDeleted = threadService.Delete(loggedId, id);

                return Ok(threadDeleted);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }


        [HttpPut("rate/{threadId}")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Rate(int threadId, [FromQuery] int rating)
        {
            try
            {
                string userIdStr = User.FindFirstValue("UserID");
                int userId = int.Parse(userIdStr);
                var ratedThread = threadService.AddRating(threadId, rating, userId);
                ThreadRatedResponseDto threadResponse = modelMapper.MapRatedThread(ratedThread, rating);

                return StatusCode(StatusCodes.Status200OK, threadResponse);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
