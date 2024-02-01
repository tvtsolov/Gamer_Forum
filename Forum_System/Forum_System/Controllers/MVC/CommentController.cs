using Forum_System.Models.ViewМodels;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Forum_System.Exceptions;
using Forum_System.Models;
using Forum_System.Helpers;
using Forum_System.Models.QueryParameters;

namespace Forum_System.Controllers.MVC
{
    public class CommentController : Controller
    {
        private readonly ICommentService commentService;
        private readonly IThreadService threadService;
        private readonly IAuthService authService;

        public CommentController(ICommentService commentService, IAuthService authService, IThreadService threadService)
        {
            this.commentService = commentService;
            this.authService = authService;
            this.threadService = threadService;
        }

        [IsAuthenticated]
        public IActionResult Index(CommentQueryParameters queryParameters)
        {
            ViewData["SortOrder"] = string.IsNullOrEmpty(queryParameters.SortOrder) ? "desc" : "";
            var comments = commentService.FilterBy(queryParameters);

            return View(comments);
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Create([FromRoute] int id)
        {
            ViewData["ThreadId"] = id;

            //Maybe rewrite
            var thread = threadService.GetById(id);
            string threadTitle = thread.Title;

            ViewData["ThreadTitle"] = threadTitle;

            var commentCreateVM = new CommentCreateViewModel()
            {
                ThreadId = id
            };

            return View(commentCreateVM);
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult Create([FromRoute] int id, CommentCreateViewModel commentCreateVM)
        {
            if (!ModelState.IsValid)
            {
                commentCreateVM.ThreadId = id;
                return View(commentCreateVM);
            }
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                var newComment = commentService.Create(loggedId, commentCreateVM);

                return RedirectToAction("Details", "Thread", new { id = commentCreateVM.ThreadId });
            }
            catch (InvalidUserInputException e)
            {
                ModelState.AddModelError("Content", e.Message);

                return View(commentCreateVM);
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var comment = commentService.GetById(id);

                return View(comment);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                var comment = commentService.GetById(id);

                commentService.Delete(loggedId, id);

                int threadId = comment.ThreadId;

                return RedirectToAction("Details", "Thread", new {id = threadId});

            }
            catch (UnauthorizedAccessException ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
            catch (EntityNotFoundException ex) // mb useless?
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }
    }
}
