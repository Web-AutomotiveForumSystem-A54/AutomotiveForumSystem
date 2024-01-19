using Microsoft.AspNetCore.Mvc;

using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostModelMapper mapper;
        private readonly IPostService postService;
		private readonly ICategoriesService categoriesService;
        private readonly ICategoryModelMapper categoryModelMapper;

        public UsersController(IUsersService usersService, 
            IPostModelMapper mapper, 
            IPostService postService,
            ICategoriesService categoriesService,
            ICategoryModelMapper categoryModelMapper
            )
        {
            this.usersService = usersService;
            this.mapper = mapper;
            this.postService = postService;
            this.categoriesService = categoriesService;
            this.categoryModelMapper = categoryModelMapper;
        }

        [HttpGet]
        public IActionResult Index([FromQuery]string username)
        {
            var user = this.usersService.GetByUsername(username);
            var posts = this.postService.GetPostsByUser(user.Id, new PostQueryParameters());

            var allCategories = GlobalQueries.InitializeCategoriesFromDatabase(this.categoriesService);
            var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

            ViewData["CategoryLabels"] = categoryLabels;
            ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
            ViewData["MembersCount"] = this.usersService.GetAll().Count;

            var model = new UserProfileViewModel
            {
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Posts = this.mapper.MapPostsToPreViewModel(posts)
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] string username)
        {
            var user = this.usersService.GetByUsername(username);
            var allCategories = GlobalQueries.InitializeCategoriesFromDatabase(this.categoriesService);
            var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

            ViewData["CategoryLabels"] = categoryLabels;
            ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
            ViewData["MembersCount"] = this.usersService.GetAll().Count;

            var model = new UserProfileEditInfoViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(model);
        }

        //[HttpPost]
        //public IActionResult Edit(UserProfileEditInfoViewModel model)
        //{
        //    try
        //    {
        //        var user = this.usersService.GetByUsername(HttpContext.Session.GetString("CurrentUsername"));

        //        this.usersService.UpdateProfileInformation

        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }
}
