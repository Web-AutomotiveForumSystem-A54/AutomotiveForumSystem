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
			try
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
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult Search(PostQueryParameters postQueryParameters)
		{
			try
			{
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
							usersService, postService, categoryModelMapper);

				ViewData["SearchQuery"] = postQueryParameters.Title;
				var posts = this.postService.GetAll(postQueryParameters);
				var postViewModelList = this.postModelMapper.MapPostsToPreViewModel(posts);
				return View(postViewModelList);
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
		}

		[HttpPost]
		public IActionResult CreateComment(int postId, PostDataViewModel postModel)
		{
			var blockedResult = CheckUserBlockedStatus();
			if (blockedResult != null)
			{
				return blockedResult;
			}

			try
			{
				var user = usersService.GetByUsername(HttpContext.Session.GetString("CurrentUser"));
				var post = postService.GetPostById(postId);

				if (string.IsNullOrEmpty(postModel.Comment.Content) || postModel.Comment.Content.Length > 8192)
				{
					GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
						usersService, postService, categoryModelMapper);

					ViewData["ErrorMessage"] = "Content must be between 1 and 8192 characters.";
					return View("Error", postModel.Comment.Content);
				}

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
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpPost]
		public IActionResult UpdateComment([FromQuery] int commentId, PostDataViewModel postModel)
		{
			var blockedResult = CheckUserBlockedStatus();
			if (blockedResult != null)
			{
				return blockedResult;
			}

			try
			{
				var comment = this.commentsService.GetCommentById(commentId);
				var postId = comment.PostID;
				var user = comment.User;
				this.commentsService.UpdateComment(user, commentId, postModel.Comment.Content);

				return RedirectToAction("Index", "Posts", new { id = postId });
			}
			catch (AuthorizationException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult DeleteComment([FromQuery] int postId, [FromQuery] int commentId)
		{
			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			var blockedResult = CheckUserBlockedStatus();
			if (blockedResult != null)
			{
				return blockedResult;
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
			try
			{
				if (!HttpContext.Session.Keys.Contains("CurrentUser"))
				{
					return RedirectToAction("Login", "Auth");
				}
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
						usersService, postService, categoryModelMapper);

				var blockedResult = CheckUserBlockedStatus();
				if (blockedResult != null)
				{
					return blockedResult;
				}

				var postCreateModel = new PostCreateViewModel();
				InitializeCategoriesInViewModel(postCreateModel);
				return View(postCreateModel);
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
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


			if (!ModelState.IsValid)
			{
				InitializeCategoriesInViewModel(postCreateViewModel);
				return View(postCreateViewModel);
			}

			try
			{
				var currentUser = HttpContext.Session.GetString("CurrentUser");

				List<Tag> tags = new List<Tag>();

				if (postCreateViewModel.Tags != null)
				{
					var inputTags = postCreateViewModel.Tags.Split(' ');
					foreach (var item in inputTags)
					{
						var _sent_tag = item.ToLower();

						var tag = tagsService.GetByName(_sent_tag);
						if (tag != null)
						{
							tags.Add(tagsService.GetByName(_sent_tag));
						}
						else
						{
							Tag _tag = new Tag()
							{
								Name = _sent_tag,
							};
							var newTag = tagsService.Create(_tag);
							tags.Add(_tag);
						}
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
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult Edit([FromRoute] int id)
		{
			try
			{
				if (!HttpContext.Session.Keys.Contains("CurrentUser"))
				{
					return RedirectToAction("Login", "Auth");
				}
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
						usersService, postService, categoryModelMapper);


				var post = this.postService.GetPostById(id);
				var tagNames = string.Join(" ", post.Tags.Select(tag => tag.Name));
				var postEditViewModel = new PostEditViewModel()
				{
					Id = post.Id,
					Title = post.Title,
					Content = post.Content,
					CategoryID = post.CategoryID,
					UserID = post.UserID,
					Tags = tagNames
				};
				InitializeCategoriesInViewModel(postEditViewModel);

				return View(postEditViewModel);
			}
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpPost]
		public IActionResult Edit([FromRoute] int id, PostEditViewModel postEditViewModel)
		{
			try
			{
				GlobalQueries.InitializeLayoutBasedData(this, categoriesService, tagsService,
							usersService, postService, categoryModelMapper);

				if (!ModelState.IsValid)
				{
					InitializeCategoriesInViewModel(postEditViewModel);
					return View(postEditViewModel);
				}
				var post = this.postService.GetPostById(id);
				var updatedPost = new Post()
				{
					Title = postEditViewModel.Title,
					Content = postEditViewModel.Content,
					CategoryID = postEditViewModel.CategoryID
				};
				this.postService.Update(id, updatedPost, post.User);
				return RedirectToAction("Index", "Posts", new { id });
			}
			catch (AuthorizationException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult DeletePost([FromQuery] int postId)
		{
			try
			{
				var currentUsername = HttpContext.Session.GetString("CurrentUser");
				var user = this.usersService.GetByUsername(currentUsername);
				this.postService.DeletePost(postId, user);
				return RedirectToAction("Index", "Home");
			}
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult LikePost([FromQuery] int postId)
		{
			try
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
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		[HttpGet]
		public IActionResult RemoveLike([FromQuery] int postId)
		{
			try
			{
				var currentUsername = HttpContext.Session.GetString("CurrentUser");
				var user = this.usersService.GetByUsername(currentUsername);

				this.likesService.RemoveLike(postId, user.Id);
				return RedirectToAction("Index", "Posts", new { id = postId });
			}
			catch (EntityNotFoundException ex)
			{
				ViewData["ErrorMessage"] = ex.Message;
				return View("Error");
			}
			catch (Exception ex)
			{
				ViewData["ErrorMessage"] = "Unexpected error: " + ex.Message;
				return View("Error");
			}
		}

		private void InitializeCategoriesInViewModel(PostCreateViewModel postCreateViewModel)
		{
			postCreateViewModel.Categories = new SelectList(this.categoriesService.GetAll(), "Id", "Name");
		}

		private void InitializeCategoriesInViewModel(PostEditViewModel postEditViewModel)
		{
			postEditViewModel.Categories = new SelectList(this.categoriesService.GetAll(), "Id", "Name");
		}

		private IActionResult CheckUserBlockedStatus()
		{
			var currentUser = HttpContext.Session.GetString("CurrentUser");
			var user = this.usersService.GetByUsername(currentUser);

			if (user.IsBlocked)
			{
				ViewData["ErrorMessage"] = "You are blocked from creating new posts!";
				return View("Error");
			}

			return null; // Indicates that the user is not blocked
		}
	}
}
