using Microsoft.AspNetCore.Mvc;

using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.PostDtos;

namespace AutomotiveForumSystem.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IPostModelMapper mapper;
        private readonly IPostService postService;

        public UsersController(IUsersService usersService, IPostModelMapper mapper, IPostService postService)
        {
            this.usersService = usersService;
            this.mapper = mapper;
            this.postService = postService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery]string username)
        {
            var user = this.usersService.GetByUsername(username);
            var posts = this.postService.GetPostsByUser(user.Id, new PostQueryParameters());

            var model = new UserProfileViewModel
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Posts = this.mapper.MapPostsToPreViewModel(posts)
            };

            return View(model);
        }
    }
}
