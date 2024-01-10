using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICategoriesService categoriesService;
		private readonly IPostService postService;
		private readonly IPostModelMapper postModelMapper;

        public HomeController(ICategoriesService categoriesService, IPostService postService, IPostModelMapper postModelMapper)
        {
			this.categoriesService = categoriesService;
			this.postService = postService;
			this.postModelMapper = postModelMapper;
        }

        [HttpGet]
		public IActionResult Index()
		{
			PostQueryParameters postQueryParameters = new PostQueryParameters();
			try
			{
				var allCategories = categoriesService.GetAll();
				var categoryLabels = new List<CategoryLabelViewModel>();
				foreach (var category in allCategories)
				{
					categoryLabels.Add(new CategoryLabelViewModel()
					{
						Name = category.Name,
						PostsCount = category.Posts.Count
					});
				}

				ViewData["CategoryLabels"] = categoryLabels;

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
