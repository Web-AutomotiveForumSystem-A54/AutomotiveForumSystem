using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
    public class AdminsController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostService postService;
        private readonly ICategoriesService categoriesService;
        private readonly ICategoryModelMapper categoryModelMapper;
        private readonly ITagsService tagsService;

        public AdminsController(IUsersService usersService,
            IPostService postService,
            ICategoriesService categoriesService,
            ICategoryModelMapper categoryModelMapper,
            ITagsService tagsService
            )
        {
            this.usersService = usersService;
            this.postService = postService;
            this.categoriesService = categoriesService;
            this.categoryModelMapper = categoryModelMapper;
            this.tagsService = tagsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var model = new AdminSearchViewModel();
            return View(model);
        }


        [HttpGet]
        public IActionResult Search(AdminSearchViewModel model)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            try
            {
                if (!string.IsNullOrEmpty(model.Username))
                {
                    var user = this.usersService.GetByUsername(model.Username);
                    return RedirectToAction("Index", "Users", new { username = user.Username });
                }
                else if (!string.IsNullOrEmpty(model.Email))
                {
                    var user = this.usersService.GetByEmail(model.Email);
                    return RedirectToAction("Index", "Users", new { username = user.Username });
                }
                else if (!string.IsNullOrEmpty(model.FirstName))
                {
                    var users = this.usersService.GetByFirstName(model.FirstName);
                    return View("UsersList", users);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ViewData["ErrorMessage"] = "No such user found";
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Block([FromQuery] string username)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var user = this.usersService.GetByUsername(username);
            _ = this.usersService.Block(user);

            return RedirectToAction("Index", "Users", new { username = user.Username });
        }

        [HttpGet]
        public IActionResult Unblock([FromQuery] string username)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var user = this.usersService.GetByUsername(username);
            _ = this.usersService.Unblock(user);

            return RedirectToAction("Index", "Users", new { username = user.Username });
        }

        [HttpGet]
        public IActionResult SetAdmin([FromQuery] string username)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var user = this.usersService.GetByUsername(username);
            _ = this.usersService.SetAdmin(user);

            return RedirectToAction("Index", "Users", new { username = user.Username });
        }
    }
}
