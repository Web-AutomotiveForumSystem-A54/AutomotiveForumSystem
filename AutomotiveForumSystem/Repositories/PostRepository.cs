﻿using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Repositories.Contracts;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveForumSystem.Repositories
{
	public class PostRepository : IPostRepository
	{
		private readonly ApplicationContext applicationContext;
		private readonly ICommentsRepository commentsRepository;

		public PostRepository(ApplicationContext applicationContext, ICommentsRepository commentsRepository)
		{
			this.applicationContext = applicationContext;
			this.commentsRepository = commentsRepository;
		}

		public Post CreatePost(Post post, User currentUser)
		{
			post.CreateDate = DateTime.Now;
			this.applicationContext.Posts.Add(post);
			currentUser.Posts.Add(post);
			applicationContext.SaveChanges();

			return post;
		}

		public IList<Post> GetAll(PostQueryParameters postQueryParameters)
		{
			var postsToReturn = this.applicationContext.Posts
				.Include(p => p.Category)
				.Include(p => p.Comments)
				.Include(p => p.User)
				.Where(p => !p.IsDeleted)
				.AsEnumerable();

			if (!string.IsNullOrEmpty(postQueryParameters.Category))
			{
				postsToReturn = postsToReturn.Where(p => p.Category.Name == postQueryParameters.Category);
			}

			if (!string.IsNullOrEmpty(postQueryParameters.Title))
			{
				string[] titleKeywords = postQueryParameters.Title.Split(' ');
				postsToReturn = postsToReturn.Where(p => titleKeywords.All(keyword => p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
			}
			if (!string.IsNullOrEmpty(postQueryParameters.Tag))
			{
				postsToReturn = postsToReturn.Where(p => p.Tags.Any(t => t.Name == postQueryParameters.Tag));
			}
			if (!string.IsNullOrEmpty(postQueryParameters.SortBy))
			{
				if (postQueryParameters.SortBy == "likes")
				{
					postsToReturn = postsToReturn.OrderByDescending(p => p.TotalLikesCount).Take(10);
				}
				else if (postQueryParameters.SortBy == "date")
				{
					postsToReturn = postsToReturn.OrderByDescending(p => p.CreateDate).Take(10);
				}
			}

			return postsToReturn.ToList();
		}

		public IList<Post> GetAll()
		{
			return this.applicationContext.Posts
				.Include(p => p.Category)
				.Include(p => p.Tags)
				.Where(p => !p.IsDeleted)
				.ToList();
		}

		public Post GetPostById(int id)
		{
			return applicationContext.Posts
				.Include(p => p.Category)
				.Include(p => p.Comments.Where(c => !c.IsDeleted))
				.Include(p => p.User)
				.Include(p => p.Tags)
				.Include(p => p.Likes.Where(l => !l.IsDeleted))
				.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
				?? throw new EntityNotFoundException($"Post with ID: {id} not found");
		}

		public IList<Post> GetPostsByUser(int userId, PostQueryParameters postQueryParameters)
		{
			var postsToReturn = applicationContext.Posts
				.Where(p => p.UserID == userId && !p.IsDeleted)
				.AsQueryable();

			if (!string.IsNullOrEmpty(postQueryParameters.Category))
			{
				postsToReturn = postsToReturn.Where(p => p.Category.Name == postQueryParameters.Category);
			}

			if (!string.IsNullOrEmpty(postQueryParameters.Title))
			{
				string[] titleKeywords = postQueryParameters.Title.Split(' ');
				postsToReturn = postsToReturn.Where(p => titleKeywords.All(keyword => p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase)));
			}
			if (!string.IsNullOrEmpty(postQueryParameters.Tag))
			{
				postsToReturn = postsToReturn.Where(p => p.Tags.Any(t => t.Name == postQueryParameters.Tag));
			}
			if (!string.IsNullOrEmpty(postQueryParameters.SortBy))
			{
				if (postQueryParameters.SortBy == "likes")
				{
					postsToReturn = postsToReturn.OrderByDescending(p => p.TotalLikesCount);
				}
				else if (postQueryParameters.SortBy == "date")
				{
					postsToReturn = postsToReturn.OrderByDescending(p => p.CreateDate);
				}
			}
			return postsToReturn.Include(p => p.Category).ToList();
		}

		public int GetTotalPostCount()
		{
			return this.applicationContext.Posts.Where(p => !p.IsDeleted).Count();
		}

		public Post UpdatePost(int id, Post updatedPost)
		{
			var postToUpdate = applicationContext.Posts.FirstOrDefault(p => p.Id == id && !p.IsDeleted)
				?? throw new EntityNotFoundException($"Post with ID: {id} not found");

			postToUpdate.Title = updatedPost.Title;
			postToUpdate.Content = updatedPost.Content;
			postToUpdate.Tags = updatedPost.Tags;

			if (postToUpdate.CategoryID != updatedPost.CategoryID)
			{
				var newCategory = applicationContext.Categories.Where(c => !c.IsDeleted).FirstOrDefault(c => c.Id == updatedPost.CategoryID)
					?? throw new EntityNotFoundException($"Category with ID {updatedPost.CategoryID} not found");
				postToUpdate.CategoryID = updatedPost.CategoryID;
			}

			applicationContext.SaveChanges();

			return postToUpdate;
		}

		public void DeletePost(Post post)
		{
			post.IsDeleted = true;
			commentsRepository.DeleteComments(post.Comments);
			applicationContext.SaveChanges();
		}


	}
}
