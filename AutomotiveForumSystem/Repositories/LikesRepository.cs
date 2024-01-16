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
		private readonly IUsersRepository usersRepository;

		public LikesRepository(ApplicationContext applicationContext, IPostRepository postRepository, IUsersRepository usersRepository)
		{
			this.applicationContext = applicationContext;
			this.postRepository = postRepository;
			this.usersRepository = usersRepository;
		}

		public void LikePost(Like like, int postId, int userId)
		{
			applicationContext.Likes.Add(like);
			var post = this.postRepository.GetPostById(postId);

			var user = this.usersRepository.GetById(userId);

			post.Likes.Add(like);
			user.Likes.Add(like);
			post.TotalLikesCount++;

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
			//var like = this.applicationContext.Likes
			//	.Include(l => l.User)
			//	.Include(l => l.Post)
			//	.FirstOrDefault(l => l.PostId == postId && l.UserId == userId && !l.IsDeleted);
			var like = post.Likes.FirstOrDefault(l => l.UserId == userId);
			like.IsDeleted = true;
			this.applicationContext.SaveChanges();
		}
	}
}
