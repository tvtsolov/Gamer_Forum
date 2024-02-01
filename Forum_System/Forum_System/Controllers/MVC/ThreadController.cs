using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models.ViewМodels;
using Forum_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Forum_System.Models;
using Forum_System.Models.QueryParameters;
using System.Threading;
using Microsoft.IdentityModel.Tokens;

namespace Forum_System.Controllers.MVC
{
    public class ThreadController : Controller
    {
        private readonly IThreadService threadService;
        private readonly ICommentService commentService;
        private readonly ModelMapper mapper;

        public ThreadController(IThreadService threadService, ModelMapper mapper, ICommentService commentService)
        {
            this.threadService = threadService;
            this.commentService = commentService;
            this.mapper = mapper;
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Index(ThreadQueryParameters queryParameters)
        {
            ViewData["SortOrder"] = string.IsNullOrEmpty(queryParameters.SortOrder) ? "desc" : "";
            ViewData["AuthorSearch"] = string.IsNullOrEmpty(queryParameters.Author) ? "" : queryParameters.Author;
            ViewData["TitleSearch"] = string.IsNullOrEmpty(queryParameters.Title) ? "" : queryParameters.Title;
            var threads = threadService.FilterBy(queryParameters);

            return View(threads);
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Details([FromRoute] int id)
        {
            try
            {
                Thread thread = threadService.GetById(id);
                ThreadDetailsViewModel threadDetailsVM = mapper.MapThreadWithLoginID(thread);
                var currentId = HttpContext.Session.GetString("CurrentID");

                if (!currentId.IsNullOrEmpty())
                {
                    threadDetailsVM.LoggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                }
                else
                {
                    threadDetailsVM.LoggedUserId = -1;
                }
                return View(threadDetailsVM);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Create()
        {
            var threadCreateVM = new ThreadCreateViewModel();

            return View(threadCreateVM);
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult Create(ThreadCreateViewModel threadCreateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(threadCreateVM);
            }
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                var newThread = mapper.MapCreate(threadCreateVM);

                threadService.Create(loggedId, newThread);

                return RedirectToAction("Details", "Thread", new { id = newThread.Id });
            }
            catch (InvalidUserInputException e)
            {
                ModelState.AddModelError("Content", e.Message);

                return View("Index", "Home");
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
        public IActionResult EditComment([FromRoute] int id)
        {
            try
            {
                Thread thread = threadService.GetAll().Where(t => t.Comments.Count != 0)
                    .FirstOrDefault(t => t.Comments.Any(c => c.Id == id));

                Comment comment = commentService.GetById(id);
                ViewData["ThreadTitle"] = thread.Title;

                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                if(loggedUserId != comment.AuthorId)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(comment);
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return this.View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult EditComment([FromRoute] int id, [FromForm] Comment editedComment)
        {
            try
            {
                commentService.Update(id, editedComment);
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                if (loggedUserId != editedComment.AuthorId)
                {
                    throw new UnauthorizedAccessException("You are not authorized to do that");
                }

                return RedirectToAction("Details", "Thread", new { id = editedComment.ThreadId });
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;
                return this.View("Error");
            }

        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult EditThread([FromRoute] int id)
        {
            try
            {

                Thread thread = threadService.GetById(id);
                ViewData["ThreadTitle"] = thread.Title;
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                if (loggedUserId != thread.AuthorId)
                {
                    return RedirectToAction("Index", "Home");
                }

                return View(thread);
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return this.View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult EditThread([FromRoute] int id, [FromForm] Thread updatedThread)
        {
            //todo refactor
            try
            {
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));

                threadService.Update(loggedUserId, id, updatedThread.Content, updatedThread.Title); //check if user is blocked + if is the author

                return RedirectToAction("Details", "Thread", new { id = id });
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;
                return this.View("Error");
            }
        }


        [IsAuthenticated]
        [HttpGet]
        public IActionResult DeleteComment([FromRoute]int id)
        {
            try
            {
                var thread = threadService.GetAll().Where(t => t.Comments.Count != 0)
                    .FirstOrDefault(t => t.Comments.Any(c => c.Id == id)) ?? throw new EntityNotFoundException ("Comment not found");
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                string adminStatus = HttpContext.Session.GetString("AdminStatus");
                Comment comment = commentService.GetById(id);

                ViewData["ThreadTitle"] = thread.Title;

                if (comment.AuthorId != loggedUserId && adminStatus != "True")
                {
                    throw new UnauthorizedAccessException("You are not authorize to do that");
                }

                return View(comment);
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return this.View("Error");
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult DeleteComment([FromRoute] int id, [FromForm] Comment deletedComment)
        {
            try
            {
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                int deletedCommentThreadId = deletedComment.ThreadId;

                string adminStatus = HttpContext.Session.GetString("AdminStatus");

                if (deletedComment.AuthorId != loggedUserId && adminStatus != "True")
                {
                    throw new UnauthorizedAccessException("You are not authorized to do that");
                }

                commentService.Delete(id);
                
                return RedirectToAction("Details", "Thread", new { id = deletedCommentThreadId });
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return this.View("Error");
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
        public IActionResult DeleteThread([FromRoute] int id)
        {
            try
            {
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                Thread thread = threadService.GetById(id);
                string adminStatus = HttpContext.Session.GetString("AdminStatus");


                if (thread.AuthorId != loggedUserId && adminStatus != "True")
                {
                    throw new UnauthorizedAccessException("You are not authorized to do that");
                }

                return View(thread);
            }
            catch (EntityNotFoundException ex)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = ex.Message;
                return this.View("Error");
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult DeleteThread([FromRoute] int id, [FromForm] Thread deletedThread)
        {
            try
            {
                int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                threadService.Delete(loggedUserId, id);



                return RedirectToAction("Index", "Home");
            }
            catch (UnauthorizedAccessException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult AddRating([FromForm]double rating, [FromForm] int threadId)
        {
            int loggedUserId = int.Parse(HttpContext.Session.GetString("CurrentID"));
            Thread thread = threadService.GetById(threadId);
            //check if this is rating update, not a new rating

            if(thread.Ratings.Any(r => r.UserId == loggedUserId))           // user has rated this thread already
            {
                int ratingId = thread.Ratings.FirstOrDefault(r => r.UserId == loggedUserId).Id;
                threadService.TryRemoveRating(ratingId, threadId);
                threadService.AddRating(threadId, rating, loggedUserId);
            }
            else                                                            // no rating by this user currently
            {
                threadService.AddRating(threadId, rating, loggedUserId);
            }
            
            return RedirectToAction("Details", "Thread", new { id = threadId });
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult RemoveRating([FromForm]int ratingId, [FromForm] int threadId)
        {
            try
            {
                threadService.TryRemoveRating(ratingId, threadId);
                return RedirectToAction("Details", "Thread", new { id = threadId });
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

    }
}
