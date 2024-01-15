using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutomotiveForumSystem.Controllers
{
	public class PostsController : Controller
	{
		private readonly IPostService postService;
		private readonly IPostModelMapper postModelMapper;
		private readonly ICommentsService commentsService;
		private readonly ICategoriesService categoriesService;
		private readonly ICategoryModelMapper categoryModelMapper;
		private readonly IUsersService usersService;
		private readonly IAuthManager authManager;

		public PostsController(IPostService postService,
			IPostModelMapper postModelMapper,
			ICommentsService commentsService,
			ICategoriesService categoriesService,
			ICategoryModelMapper categoryModelMapper,
			IUsersService usersService,
			IAuthManager authManager)
		{
			this.postService = postService;
			this.postModelMapper = postModelMapper;
			this.commentsService = commentsService;
			this.categoriesService = categoriesService;
			this.categoryModelMapper = categoryModelMapper;
			this.usersService = usersService;
			this.authManager = authManager;
		}

		[HttpGet]
		public IActionResult Index([FromRoute] int id)
		{
			var post = this.postService.GetPostById(id);
			var postDataViewModel = this.postModelMapper.MapPostToDataViewModel(post);

			var allCategories = GlobalQueries.InitializeCategories(this.categoriesService);
			var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

			ViewData["CategoryLabels"] = categoryLabels;
			ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
			ViewData["MembersCount"] = this.usersService.GetAll().Count;

			return View(postDataViewModel);
		}

		[HttpPost]
		public IActionResult CreateComment(int postId, PostDataViewModel postModel)
		{
			var user = usersService.GetByUsername("jonkata");
			var post = postService.GetPostById(postId);
			var newComment = new Comment()
			{
				PostID = post.Id,
				UserID = user.Id,
				Content = postModel.Comment.Content,
				CreateDate = DateTime.Now,
			};
			commentsService.CreateComment(user, post, newComment, null);

			return RedirectToAction("Index", "Posts", new { id = post.Id });
		}

		[HttpPost]
		public IActionResult UpdateComment([FromQuery] int commentId, PostDataViewModel postModel)
		{

			var comment = this.commentsService.GetCommentById(commentId);
			var postId = comment.PostID;
			var user = comment.User;
			this.commentsService.UpdateComment(user, commentId, postModel.Comment.Content);


			return RedirectToAction("Index", "Posts", new { id = postId });
		}

		[HttpGet]
		public IActionResult CreatePost()
		{
			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}
			var allCategories = GlobalQueries.InitializeCategories(this.categoriesService);
			var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

			ViewData["CategoryLabels"] = categoryLabels;
			ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
			ViewData["MembersCount"] = this.usersService.GetAll().Count;
			var postCreateModel = new PostCreateViewModel();
			InitializeCategories(postCreateModel);
			return View(postCreateModel);
		}

		[HttpPost]
		public IActionResult CreatePost(PostCreateViewModel postCreateViewModel)
		{
			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			//if (!ModelState.IsValid)
			//{
			//	InitializeCategories(postCreateViewModel);
			//	return View(postCreateViewModel);
			//}

			var allCategories = GlobalQueries.InitializeCategories(this.categoriesService);
			var categoryLabels = this.categoryModelMapper.ExtractCategoriesLabels(allCategories);

			ViewData["CategoryLabels"] = categoryLabels;
			ViewData["TotalPostsCount"] = this.postService.GetTotalPostCount();
			ViewData["MembersCount"] = this.usersService.GetAll().Count;

			try
			{
				var currentUser = HttpContext.Session.GetString("CurrentUser");

				var user = this.usersService.GetByUsername(currentUser);

				var post = new Post()
				{
					Title = postCreateViewModel.Title,
					Content = postCreateViewModel.Content,
					UserID = user.Id,
					CategoryID = postCreateViewModel.CategoryID,
				};
				var createdPost = this.postService.CreatePost(post, user);

				return RedirectToAction("Index", "Posts", new { id = createdPost.Id });
			}
			catch (EntityNotFoundException)
			{
				return View(postCreateViewModel);
			}
		}

		[HttpGet]
		public IActionResult Delete([FromQuery]int postId)
		{
			var currentUsername = HttpContext.Session.GetString("CurrentUser");
			var user = this.usersService.GetByUsername(currentUsername);
			this.postService.DeletePost(postId, user);
			return RedirectToAction("Index", "Home");
		}

		private void InitializeCategories(PostCreateViewModel postCreateViewModel)
		{
			postCreateViewModel.Categories = new SelectList(this.categoriesService.GetAll(), "Id", "Name");
		}
	}
}
