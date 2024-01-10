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
		private readonly IUsersService usersService;

		public HomeController(
			ICategoriesService categoriesService,
			IPostService postService,
			IPostModelMapper postModelMapper,
			IUsersService usersService)
		{
			this.categoriesService = categoriesService;
			this.postService = postService;
			this.postModelMapper = postModelMapper;
			this.usersService = usersService;
		}

		[HttpGet]
		public IActionResult Index()
		{
			try
			{
				var allCategories = categoriesService.GetAll();
				var categoryLabels = ExtractCategoriesLabels(allCategories);

				PostQueryParameters postQueryParameters = new PostQueryParameters();

				ViewData["CategoryLabels"] = categoryLabels;
				ViewData["TotalPostsCount"] = this.postService.GetAll(postQueryParameters).Count;
				ViewData["MembersCount"] = this.usersService.GetAll().Count;

				IList<Post> posts = this.postService.GetAll(postQueryParameters);
				IList<PostDataViewModel> postsDataViewModelList = this.postModelMapper.MapPostsToDataViewModel(posts);
				return View(postsDataViewModelList);
			}
			catch (EntityNotFoundException ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult PostsByCategory([FromRoute] int id)
		{
			try
			{
				var allCategories = categoriesService.GetAll();
				var categoryLabels = ExtractCategoriesLabels(allCategories);

				PostQueryParameters postQueryParameters = new PostQueryParameters();

				ViewData["CategoryLabels"] = categoryLabels;
				ViewData["TotalPostsCount"] = this.postService.GetAll(postQueryParameters).Count;
				ViewData["MembersCount"] = this.usersService.GetAll().Count;

				var category = categoriesService.GetCategoryById(id);
				ViewData["CategoryPreview"] = category.Name;

				IList<Post> posts = category.Posts;
				IList<PostDataViewModel> postsDataViewModelList = this.postModelMapper.MapPostsToDataViewModel(posts);
				return View(postsDataViewModelList);
			}
			catch (EntityNotFoundException ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}

		private IList<CategoryLabelViewModel> ExtractCategoriesLabels(IList<Category> categories)
		{
			var categoryLabels = new List<CategoryLabelViewModel>();
			foreach (var category in categories)
			{
				categoryLabels.Add(new CategoryLabelViewModel()
				{
					Id = category.Id,
					Name = category.Name,
					PostsCount = category.Posts.Count
				});
			}

			return categoryLabels;
		}
	}
}
