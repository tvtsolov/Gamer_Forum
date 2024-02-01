using Forum_System.Exceptions;
using Forum_System.Helpers;
using Forum_System.Models.ViewМodels;
using Forum_System.Services;
using Forum_System.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Security.Authentication;

namespace Forum_System.Controllers.MVC
{
	public class AuthController : Controller
	{
		private readonly IAuthService authService;
		private readonly IUserService userService;
		private readonly ModelMapper modelMapper;

		public AuthController(IAuthService authService, IUserService userService, ModelMapper modelMapper)
		{
			this.authService = authService;
			this.userService = userService;
			this.modelMapper = modelMapper;
		}

		[HttpGet]
		public IActionResult Login()
		{
			var loginViewModel = new LoginViewModel();

			return View(loginViewModel);
		}

		[HttpPost]
		public IActionResult Login(LoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}

			try
			{
				var user = authService.TryUserCredentials(loginViewModel.Username, loginViewModel.Password);
				HttpContext.Session.SetString("CurrentUser", user.Username);
				HttpContext.Session.SetString("CurrentID", user.Id.ToString());
				HttpContext.Session.SetString("AdminStatus", user.IsAdmin.ToString());

				return RedirectToAction("Index", "Home");
			}
			catch (InvalidCredentialsException e)
			{
				ModelState.AddModelError("Username", e.Message);
				ModelState.AddModelError("Password", e.Message);

				return View(loginViewModel);
			}

		}

        [IsAuthenticated]
        [HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.Remove("CurrentUser");
			HttpContext.Session.Remove("CurrentID");
			HttpContext.Session.Remove("AdminStatus");

			return RedirectToAction("Index", "Home");
		}

        [HttpGet]
        public IActionResult Register()
        {
            var registerViewModel = new RegisterViewModel();

            return View(registerViewModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            try
            {
                var newUser = modelMapper.MapCreate(registerViewModel);
                var createdUser = userService.Create(newUser);

                return RedirectToAction("Login", "Auth");
            }
            catch (DuplicateEntityException e)
            {
                ModelState.AddModelError("Username", e.Message);

                return View(registerViewModel);
            }
        }


    }
}
