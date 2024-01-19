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
		private readonly ITagsService tagsService;
		private readonly IPostService postService;
		private readonly IPostModelMapper postModelMapper;
		private readonly IUsersService usersService;
		private readonly ICategoryModelMapper categoryModelMapper;

		public HomeController(
			ICategoriesService categoriesService,
			ITagsService tagsService,
			IPostService postService,
			IPostModelMapper postModelMapper,
			IUsersService usersService,
			ICategoryModelMapper categoryModelMapper)
		{
			this.categoriesService = categoriesService;
			this.tagsService = tagsService;
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
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

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
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

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
