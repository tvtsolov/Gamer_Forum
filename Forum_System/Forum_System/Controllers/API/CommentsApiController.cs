using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.DTOs;
using Forum_System.Models.QueryParameters;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Forum_System.Controllers.API
{
    [Authorize]
    [ApiController]
    [Route("api/comments")]
    public class CommentsApiController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly IUserService userService;
        private readonly IAuthService authService;
        private readonly ModelMapper mapper;

        public CommentsApiController(ICommentService commentService, IUserService userService, IAuthService authService, ModelMapper mapper)
        {
            this.commentService = commentService;
            this.userService = userService;
            this.authService = authService;
            this.mapper = mapper;
        }

        [HttpGet("")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult GetAll([FromQuery] CommentQueryParameters filterParameters)
        {
            try
            {
                var comments = commentService.FilterBy(filterParameters);
                return Ok(comments);
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
                var comment = commentService.GetById(id);

                return Ok(comment);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("")]
        [Authorize(Roles = "User, Admin")]
        public IActionResult Create([FromBody] CommentCreateDto commentDto)
        {
            try
            {
                var loggedId = authService.GetLoggedUserId();
                var createdComment = commentService.Create(loggedId, commentDto);

                return StatusCode(StatusCodes.Status201Created, createdComment);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }

        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User, Admin")] // todo add check if User is the author
        public IActionResult Update(int id, [FromBody] CommentUpdateDto commentDto)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                var updatedComment = commentService.Update(loggedId, id, commentDto);
                return Ok(updatedComment);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "User, Admin")] // todo add check if User is the author
        public IActionResult Delete(int id)
        {
            try
            {
                int loggedId = authService.GetLoggedUserId();
                bool commentDeleted = commentService.Delete(loggedId, id);

                return Ok(commentDeleted);
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


    }
}
