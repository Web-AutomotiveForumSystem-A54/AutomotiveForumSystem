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

            if (string.IsNullOrEmpty(model.Username)
                && string.IsNullOrEmpty(model.Email)
                && string.IsNullOrEmpty(model.FirstName))
            {
                return RedirectToAction(nameof(Index));
            }

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

            ViewData["ErrorMessage"] = "No such user found";
            return View("Error");
        }
    }
}
