using Forum_System.Models.ViewМodels;
using Forum_System.Services;
using Microsoft.AspNetCore.Mvc;
using Forum_System.Helpers;

namespace Forum_System.Controllers.MVC
{
    public class HomeController : Controller
    {
        private readonly IThreadService threadService;
        private readonly IUserService userService;
        private readonly ICommentService commentService;
        public HomeController(IThreadService threadService, IUserService userService, ICommentService commentSerive)
        {
            this.threadService = threadService;
            this.userService = userService;
            this.commentService = commentSerive;
        }


        [HttpGet]
        public IActionResult Index()
        {
            HomeIndexViewModel homeIndexLists = new HomeIndexViewModel();

            homeIndexLists.ThreadsByDate = threadService.GetAll().OrderByDescending(t => t.Date).Take(10).ToList();
            homeIndexLists.ThreadsByComments = threadService.GetAll().OrderByDescending(t => t.Comments.Count).Take(10).ToList();
            homeIndexLists.TotalNumberOfThreads = threadService.GetAll().Count;
            homeIndexLists.TotalNumberOfUsers = userService.GetAll().Count;
            homeIndexLists.LastComment = commentService.GetAll().OrderBy(c=>c.CreationDate).Last();

            return View(homeIndexLists);
        }

        [HttpGet]
        public IActionResult About()
        {
            return View();
        }
    }


}
