using AutomotiveForumSystem.Data;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveForumSystem.Repositories
{
	public class LikesRepository : ILikesRepository
	{
		private readonly ApplicationContext applicationContext;
		private readonly IPostRepository postRepository;

		public LikesRepository(ApplicationContext applicationContext, IPostRepository postRepository)
		{
			this.applicationContext = applicationContext;
			this.postRepository = postRepository;
		}

		public void LikePost(Like like, int postId, int userId)
		{
			var post = this.postRepository.GetPostById(postId);

			var postLike = post.Likes.FirstOrDefault(l => l.UserId == userId);
			if (postLike == null)
			{
				applicationContext.Likes.Add(like);
			}
			else
			{
				postLike.IsDeleted = false;
			}
			post.TotalLikesCount = post.Likes.Where(l => l.IsDeleted == false).Count();
			this.applicationContext.SaveChanges();

		}
		public Like GetLikeByID(int id)
		{
			return applicationContext.Likes.Include(l => l.User).Include(l => l.Post).FirstOrDefault(l => l.Id == id && !l.IsDeleted)
				?? throw new EntityNotFoundException($"Like with ID: {id} not found");
		}

		public void RemoveLike(int postId, int userId)
		{
			var post = this.postRepository.GetPostById(postId);
			var like = post.Likes.FirstOrDefault(l => l.UserId == userId) ?? throw new EntityNotFoundException($"Like not found");
			like.IsDeleted = true;
			post.TotalLikesCount = post.Likes.Where(l => l.IsDeleted == false).Count();
			this.applicationContext.SaveChanges();
		}
	}
}
