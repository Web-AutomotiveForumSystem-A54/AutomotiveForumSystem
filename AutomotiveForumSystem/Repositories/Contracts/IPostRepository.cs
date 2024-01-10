using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;

namespace AutomotiveForumSystem.Repositories.Contracts
{
    public interface IPostRepository
    {
        IList<Post> GetAll(PostQueryParameters postQueryParameters);
        IList<Post> GetAll();
		IList<Post> GetPostsByUser(int userId, PostQueryParameters postQueryParameters);
        Post GetPostById(int id);
        public int GetTotalPostCount();
		Post CreatePost(Post post, User currentUser);
        Post UpdatePost(int id, Post post);
        void DeletePost(Post post);
    }
}
