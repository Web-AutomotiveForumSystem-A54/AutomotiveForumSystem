using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostModelMapper postMapper;
        private readonly IPostService postService;
        private readonly ICategoriesService categoriesService;
        private readonly ICategoryModelMapper categoryModelMapper;
        private readonly IUserMapper userMapper;
        private readonly ITagsService tagsService;

        public UsersController(IUsersService usersService,
            IPostModelMapper postMapper,
            IPostService postService,
            ICategoriesService categoriesService,
            ICategoryModelMapper categoryModelMapper,
            IUserMapper userMapper,
            ITagsService tagsService
            )
        {
            this.usersService = usersService;
            this.postMapper = postMapper;
            this.postService = postService;
            this.categoriesService = categoriesService;
            this.categoryModelMapper = categoryModelMapper;
            this.userMapper = userMapper;
            this.tagsService = tagsService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] string username)
        {
            var user = this.usersService.GetByUsername(username);
            var posts = this.postService.GetPostsByUser(user.Id, new PostQueryParameters());

            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var model = new UserProfileViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Posts = this.postMapper.MapPostsToPreViewModel(posts),
                IsAdmin = user.IsAdmin
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] string username)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            var user = this.usersService.GetByUsername(username);            

            var model = new UserUpdateProfileInformationViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit([FromQuery] string username, UserUpdateProfileInformationViewModel model)
        {
            GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
                    usersService, postService, categoryModelMapper);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {

                var user = this.usersService.GetByUsername(username);

                user = this.usersService.UpdateProfileInformation(user.Id, this.userMapper.Map(model));

                return RedirectToAction("Index", "Users", new { username = user.Username });
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Email", e.Message);
                return View(model);
            }
        }
    }
}
