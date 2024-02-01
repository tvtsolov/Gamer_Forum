using Forum_System.Exceptions;
using Forum_System.Models.QueryParameters;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum_System.Controllers.API
{
	[ApiController]
	[Route("api/tags")]
	public class TagsApiController : ControllerBase
	{
		private readonly ITagService tagService;
		private readonly IAuthService authService;

		public TagsApiController(ITagService tagService, IAuthService authService)
		{
			this.tagService = tagService;
			this.authService = authService;
		}

		//Create should be somehow implemented through the threads controller
		//Should tags be updateable? Maybe only by admin?

		[HttpGet("")]
		public IActionResult GetAll()
		{
			try
			{
				var tags = tagService.GetAll();

				return Ok(tags);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			try
			{
				var tag = tagService.GetById(id);

				return Ok(tag);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}

		//Tags maybe shouldn't be deletable?
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				int loggedId = authService.GetLoggedUserId();
				bool tagToDelete = tagService.Delete(loggedId, id);

				return Ok(tagToDelete);
			}
			catch (EntityNotFoundException e)
			{
				return NotFound(e.Message);
			}
		}
	}
}
