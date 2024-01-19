using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
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
		private readonly ITagsService tagsService;
		private readonly ICommentsService commentsService;
		private readonly ICategoriesService categoriesService;
		private readonly ICategoryModelMapper categoryModelMapper;
		private readonly IUsersService usersService;
		private readonly IAuthManager authManager;
		private readonly ILikesService likesService;

		public PostsController(IPostService postService,
			IPostModelMapper postModelMapper,
			ITagsService tagsService,
			ICommentsService commentsService,
			ICategoriesService categoriesService,
			ICategoryModelMapper categoryModelMapper,
			IUsersService usersService,
			IAuthManager authManager,
			ILikesService likesService)
		{
			this.postService = postService;
			this.postModelMapper = postModelMapper;
			this.tagsService = tagsService;
			this.commentsService = commentsService;
			this.categoriesService = categoriesService;
			this.categoryModelMapper = categoryModelMapper;
			this.usersService = usersService;
			this.authManager = authManager;
			this.likesService = likesService;
		}

		[HttpGet]
		public IActionResult Index([FromRoute] int id)
		{
			GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

			var post = this.postService.GetPostById(id);
			var userName = HttpContext.Session.GetString("CurrentUser");
			var postDataViewModel = this.postModelMapper.MapPostToDataViewModel(post);
			if (userName != null)
			{
				var user = this.usersService.GetByUsername(userName);
				var like = post.Likes.FirstOrDefault(l => l.UserId == user.Id && l.IsDeleted == false);
				postDataViewModel.LikedByCurrentUser = like != null;
			}

			return View(postDataViewModel);
		}

		[HttpGet]
		public IActionResult Search(string searchQuery)
		{
			GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

			ViewData["SearchQuery"] = searchQuery;
			var postQueryParams = new PostQueryParameters()
			{
				Title = searchQuery
			};
			var posts = this.postService.GetAll(postQueryParams);
			var postViewModelList = this.postModelMapper.MapPostsToPreViewModel(posts);
			return View(postViewModelList);
		}

		[HttpPost]
		public IActionResult CreateComment(int postId, PostDataViewModel postModel)
		{
			var user = usersService.GetByUsername(HttpContext.Session.GetString("CurrentUser"));
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
		public IActionResult DeleteComment([FromQuery] int postId, [FromQuery] int commentId)
		{
			// TODO : check if user is logged in

			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			try
			{
				var userName = HttpContext.Session.GetString("CurrentUser");
				var user = usersService.GetByUsername(userName);
				var comment = commentsService.DeleteComment(user, commentId);

				return RedirectToAction("Index", "Posts", new { Id = postId });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Home");
			}
		}

		[HttpGet]
		public IActionResult CreatePost()
		{
			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

			var postCreateModel = new PostCreateViewModel();
			InitializeCategoriesInViewModel(postCreateModel);
			return View(postCreateModel);
		}



		[HttpPost]
		public IActionResult CreatePost(PostCreateViewModel postCreateViewModel)
		{
			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
					usersService, postService, categoryModelMapper);

			try
			{
				var currentUser = HttpContext.Session.GetString("CurrentUser");


				List<Tag> tags = new List<Tag>();

				if (postCreateViewModel.Tags != null)
				{
					var inputTags = postCreateViewModel.Tags.Split(' ');
					foreach (var item in inputTags)
					{
						Console.WriteLine(item);
						tags.Add(tagsService.GetByName(item));
					}
				}

				var user = this.usersService.GetByUsername(currentUser);

				var post = new Post()
				{
					Title = postCreateViewModel.Title,
					Content = postCreateViewModel.Content,
					UserID = user.Id,
					CategoryID = postCreateViewModel.CategoryID,
					Tags = tags
				};
				var createdPost = this.postService.CreatePost(post, user);

				return RedirectToAction("Index", "Posts", new { id = createdPost.Id });
			}
			catch (EntityNotFoundException)
			{
				return View(postCreateViewModel);
			}
		}

		[HttpPost]
		public IActionResult UpdatePost([FromQuery] int postId, PostDataViewModel postDataViewModel)
		{
			var post = this.postService.GetPostById(postId);
			var updatedPost = new Post()
			{
				Title = postDataViewModel.Title,
				Content = postDataViewModel.Content,
				CategoryID = postDataViewModel.CategoryId
			};
			this.postService.Update(postId, updatedPost, post.User);
			return RedirectToAction("Index", "Posts", new { id = postId });
		}

		[HttpGet]
		public IActionResult DeletePost([FromQuery] int postId)
		{
			var currentUsername = HttpContext.Session.GetString("CurrentUser");
			var user = this.usersService.GetByUsername(currentUsername);
			this.postService.DeletePost(postId, user);
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult LikePost([FromQuery] int postId)
		{
			var currentUsername = HttpContext.Session.GetString("CurrentUser");
			var user = this.usersService.GetByUsername(currentUsername);
			var like = new Like()
			{
				PostId = postId,
				UserId = user.Id,
			};

			this.likesService.LikePost(like, postId, user.Id);
			return RedirectToAction("Index", "Posts", new { id = postId });
		}

		[HttpGet]
		public IActionResult RemoveLike([FromQuery] int postId)
		{
			try
			{
				var currentUsername = HttpContext.Session.GetString("CurrentUser");
				var user = this.usersService.GetByUsername(currentUsername);

				this.likesService.RemoveLike(postId, user.Id);
			}
			catch (EntityNotFoundException)
			{

				throw;
			}
			return RedirectToAction("Index", "Posts", new { id = postId });
		}

		private void InitializeCategoriesInViewModel(PostCreateViewModel postCreateViewModel)
		{
			postCreateViewModel.Categories = new SelectList(this.categoriesService.GetAll(), "Id", "Name");
		}
	}
}
