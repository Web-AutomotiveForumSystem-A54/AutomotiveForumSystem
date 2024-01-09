using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class HomeController : Controller
	{
		private readonly IPostService postService;
		private readonly IPostModelMapper postModelMapper;

        public HomeController(IPostService postService, IPostModelMapper postModelMapper)
        {
            this.postService = postService;
			this.postModelMapper = postModelMapper;
        }

        [HttpGet]
		public IActionResult Index()
		{
			PostQueryParameters postQueryParameters = new PostQueryParameters();
			try
			{
				IList<Post> posts = this.postService.GetAll(postQueryParameters);
				IList<PostResponseDto> response = this.postModelMapper.MapPostsToResponseDtos(posts);
				return View(response);
			}
			catch (EntityNotFoundException ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}
	}
}
