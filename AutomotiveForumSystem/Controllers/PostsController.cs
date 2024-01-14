﻿using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

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

		public PostsController(IPostService postService, 
			IPostModelMapper postModelMapper,
			ICommentsService commentsService,
			ICategoriesService categoriesService, 
			ICategoryModelMapper categoryModelMapper, 
			IUsersService usersService)
        {
			this.postService = postService;
			this.postModelMapper = postModelMapper;
			this.commentsService = commentsService;
			this.categoriesService = categoriesService;
			this.categoryModelMapper = categoryModelMapper;
			this.usersService = usersService;
		}
        
		[HttpGet]
		public IActionResult Index([FromRoute]int id)
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
		public IActionResult UpdateComment([FromQuery]int commentId, PostDataViewModel postModel)
		{
			
			var comment = this.commentsService.GetCommentById(commentId);
			var postId = comment.PostID;
			var user = comment.User;
			this.commentsService.UpdateComment(user, commentId, postModel.Comment.Content);
			

			return RedirectToAction("Index", "Posts", new { id = postId });
		}
	}
}
