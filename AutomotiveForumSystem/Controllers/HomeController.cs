using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AutomotiveForumSystem.Controllers
{
	public class HomeController : Controller
	{
		private readonly ICategoriesService categoriesService;
		private readonly IPostService postService;
		private readonly IPostModelMapper postModelMapper;
		private readonly IUsersService usersService;
		private readonly ICategoryModelMapper categoryModelMapper;

		public HomeController(
			ICategoriesService categoriesService,
			IPostService postService,
			IPostModelMapper postModelMapper,
			IUsersService usersService,
			ICategoryModelMapper categoryModelMapper)
		{
			this.categoriesService = categoriesService;
			this.postService = postService;
			this.postModelMapper = postModelMapper;
			this.usersService = usersService;
			this.categoryModelMapper = categoryModelMapper;
		}

		[HttpGet]
		public IActionResult Index()
		{
			try
			{
				var allCategories = GlobalQueries.InitializeCategoriesFromDatabase(this.categoriesService);
				var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

				ViewData["CategoryLabels"] = categoryLabels;
				ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
				ViewData["MembersCount"] = this.usersService.GetAll().Count;

				IList<Post> posts = this.postService.GetAll();
				IList<PostPreViewModel> postsDataViewModelList = this.postModelMapper.MapPostsToPreViewModel(posts);
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
				var allCategories = GlobalQueries.InitializeCategoriesFromDatabase(this.categoriesService);
				var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

				ViewData["CategoryLabels"] = categoryLabels;
				ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
				ViewData["MembersCount"] = this.usersService.GetAll().Count;

				var category = categoriesService.GetCategoryById(id);
				ViewData["CategoryPreview"] = category.Name;

				IList<Post> posts = category.Posts
					.Where(p => !p.IsDeleted)
					.ToList();
				IList<PostPreViewModel> postsDataViewModelList = this.postModelMapper.MapPostsToPreViewModel(posts);
				return View(postsDataViewModelList);
			}
			catch (EntityNotFoundException ex)
			{
				ViewBag.ErrorMessage = ex.Message;
				return View("Error");
			}
		}

	}
}
