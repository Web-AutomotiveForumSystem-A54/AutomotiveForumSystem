using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;

namespace AutomotiveForumSystem.Services.Contracts
{
    public interface IPostService
    {
        IList<Post> GetAll(PostQueryParameters postQueryParameters);
        IList<Post> GetAll();
		IList<Post> GetPostsByUser(int userId, PostQueryParameters postQueryParameters);
        Post GetPostById(int id);
        public int GetTotalPostCount();
		Post CreatePost(Post post, User currentUser);
        Post Update(int id, Post post, User currentUser);
        void DeletePost(int id, User currentUser);
    }
}
