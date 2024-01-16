using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Repositories.Contracts
{
	public interface ILikesRepository
	{
		public void LikePost(Like like, int postId, int userId);
		public Like GetLikeByID(int id);
		public void RemoveLike(int postId, int userId);
	}
}
