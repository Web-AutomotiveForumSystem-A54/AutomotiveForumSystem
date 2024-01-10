using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Repositories;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository postRepository;

        public PostService(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }
        public Post CreatePost(Post post, User currentUser)
        {
            ValidateUserNotBlocked(currentUser);
            this.postRepository.CreatePost(post, currentUser);
            return post;
        }

        public void DeletePost(int id, User currentUser)
        {
            var postToDelete = this.postRepository.GetPostById(id);
            ValidateUserAuthorization(postToDelete, currentUser);
            this.postRepository.DeletePost(postToDelete);
        }

        public IList<Post> GetAll(PostQueryParameters postQueryParameters)
        {
            return this.postRepository.GetAll(postQueryParameters);
        }

		public IList<Post> GetAll()
		{
			return this.postRepository.GetAll();
		}

		public Post GetPostById(int id)
        {
            return this.postRepository.GetPostById(id);
        }

        public IList<Post> GetPostsByUser(int userId, PostQueryParameters postQueryParameters)
        {
            return this.postRepository.GetPostsByUser(userId, postQueryParameters);
        }

		public int GetTotalPostCount()
		{
			return this.postRepository.GetTotalPostCount();
		}

		public Post Update(int id, Post post, User currentUser)
        {
            var postToUpdate = this.postRepository.GetPostById(id);
            ValidateUserPostCreator(postToUpdate, currentUser);
            return this.postRepository.UpdatePost(id, post);
        }

        private void ValidateUserNotBlocked(User currentUser)
        {
            if (currentUser.IsBlocked)
            {
                throw new UserBlockedException($"User {currentUser.UserName} is currently blocked");
            }
        }

        private void ValidateUserAuthorization(Post post, User currentUser)
        {
            if (!IsUserAdmin(currentUser) && !IsPostCreatedByUser(post, currentUser))
            {
                throw new AuthorizationException("Not admin or post creator!");
            }
        }

        private void ValidateUserPostCreator(Post post, User currentUser)
        {
            if (!IsPostCreatedByUser(post, currentUser))
            {
                throw new AuthorizationException("Not post creator!");
            }
        }

        private bool IsPostCreatedByUser(Post post, User currentUser)
        {
            return post.UserID == currentUser.Id;
        }

        private bool IsUserAdmin(User currentUser)
        {
            return currentUser.IsAdmin;
        }
    }
}
