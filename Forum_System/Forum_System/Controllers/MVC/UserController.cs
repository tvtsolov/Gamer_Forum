using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models;
using Forum_System.Models.QueryParameters;
using Forum_System.Models.ViewМodels;
using Forum_System.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Forum_System.Controllers.MVC
{
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ModelMapper mapper;

        public UserController(IUserService userService, ModelMapper mapper)
        {
            this.userService = userService;
            this.mapper = mapper;
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Index(UserQueryParameters
            queryParameters)
        {
            ViewData["SortOrder"] = string.IsNullOrEmpty(queryParameters.SortOrder) ? "desc" : "";

            var users = userService.FilterBy(queryParameters);

            return View(users);
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult Details([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
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
        public IActionResult Edit([FromRoute] int id) //Maybe remove id
        {
            try
            {
                var user = userService.GetById(id);
                var updateVM = new UserUpdateViewModel
                {
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Password = user.Password
                };

                return View(updateVM);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UserUpdateViewModel updateVM)
        {
            if (!ModelState.IsValid)
            {
                return View(updateVM);
            }
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                var user = mapper.MapUpdate(updateVM);

                string loggedName = HttpContext.Session.GetString("CurrentUser");

                if (!updateVM.Username.IsNullOrEmpty())
                {
                    if (updateVM.Username != loggedName)
                    {
                        HttpContext.Session.SetString("CurrentUser", updateVM.Username);
                    }
                }


                var updatedUser = userService.Update(loggedId, id, user);

                return RedirectToAction("Details", "User", new { id = updatedUser.Id });
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
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
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
                userService.Delete(loggedId, id);

                return RedirectToAction("Index", "User");

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

        [IsAuthenticated]
        [IsAdmin]
        [HttpGet]
        public IActionResult Promote([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpPost, ActionName("Promote")]
        public IActionResult PromoteConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                userService.Promote(loggedId, id);

                return RedirectToAction("Details", "User", new { id = id });
            }
            catch (UnauthorizedAccessException ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpGet]
        public IActionResult Demote([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpPost, ActionName("Demote")]
        public IActionResult DemoteConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                userService.Demote(loggedId, id);

                return RedirectToAction("Details", "User", new { id = id });
            }
            catch (UnauthorizedAccessException ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpGet]
        public IActionResult Block([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpPost, ActionName("Block")]
        public IActionResult BlockConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                userService.Block(loggedId, id);

                return RedirectToAction("Details", "User", new { id = id });
            }
            catch (UnauthorizedAccessException ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpGet]
        public IActionResult Unblock([FromRoute] int id)
        {
            try
            {
                var user = userService.GetById(id);

                return View(user);
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [IsAdmin]
        [HttpPost, ActionName("Unblock")]
        public IActionResult UnblockConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                userService.Unblock(loggedId, id);

                return RedirectToAction("Details", "User", new { id = id });
            }
            catch (UnauthorizedAccessException ex)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = ex.Message;

                return View("Error");
            }
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult ChangeAvatar()
        {
            var avatarVM = new AvatarViewModel();
            return View(avatarVM);
        }

        [IsAuthenticated]
        [HttpGet]
        public IActionResult RemoveAvatar([FromRoute] int id)
        {
            var user = userService.GetById(id);
            ViewData["AvatarUsername"] = user.Username;

            return View();
        }

        [IsAuthenticated]
        [HttpPost]
        public IActionResult ChangeAvatar(AvatarViewModel avatarVM)
        {
            var username = HttpContext.Session.GetString("CurrentUser");
            var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));

            userService.ChangeAvatar(username, loggedId, avatarVM);

            return RedirectToAction("Details", "User", new { id = loggedId });
        }

        [IsAuthenticated]
        [HttpPost, ActionName("RemoveAvatar")]
        public IActionResult RemoveAvatarConfirmed([FromRoute] int id)
        {
            try
            {
                var loggedId = int.Parse(HttpContext.Session.GetString("CurrentID"));
                userService.RemoveAvatar(id, loggedId);

                return RedirectToAction("Details", "User", new { id = id });
            }
            catch (EntityNotFoundException e)
            {
                Response.StatusCode = StatusCodes.Status404NotFound;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }
            catch (UnauthorizedOperationException e)
            {
                Response.StatusCode = StatusCodes.Status401Unauthorized;
                ViewData["ErrorMessage"] = e.Message;

                return View("Error");
            }

        }
    }
}
