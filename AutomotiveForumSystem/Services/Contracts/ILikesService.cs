using AutomotiveForumSystem.Models;

namespace AutomotiveForumSystem.Services.Contracts
{
	public interface ILikesService
	{
		public void LikePost(Like like, int postId, int userId);
		public Like GetLikeByID(int id);
		public void RemoveLike(int postId, int userId);
	}
}
