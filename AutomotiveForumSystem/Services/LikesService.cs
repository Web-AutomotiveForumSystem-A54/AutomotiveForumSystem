using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Services
{
	public class LikesService : ILikesService
	{
		private readonly ILikesRepository likeRepository;

		public LikesService(ILikesRepository likeRepository)
        {
			this.likeRepository = likeRepository;
		}
        public Like GetLikeByID(int id)
		{
			return this.likeRepository.GetLikeByID(id);
		}

		public void LikePost(Like like, int postId, int userId)
		{
			this.likeRepository.LikePost(like, postId, userId);
		}

		public void RemoveLike(int postId, int userId)
		{
			this.likeRepository.RemoveLike(postId, userId);
		}
	}
}
