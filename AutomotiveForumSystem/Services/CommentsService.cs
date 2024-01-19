using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Repositories.Contracts;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ICommentsRepository commentsRepository;
        private readonly IPostRepository postRepository;

        public CommentsService(ICommentsRepository commentsRepository, IPostRepository postRepository)
        {
            this.commentsRepository = commentsRepository;
            this.postRepository = postRepository;
        }

        public IList<Comment> GetAllComments(CommentQueryParameters commentQueryParameters)
        {
            return this.commentsRepository.GetAllComments(commentQueryParameters);
        }

        public Comment GetCommentById(int id)
        {
            return this.commentsRepository.GetCommentById(id);
        }

        public IList<Comment> GetAllRepliesByCommentId(int id)
        {
            return this.commentsRepository.GetAllRepliesByCommentId(id);
        }

        public Comment CreateComment(User user, Post post, Comment comment, int? baseCommentID)
        {
            if (user.IsDeleted)
            {
                throw new EntityNotFoundException($"User with user name {user.Username} not found.");
            }

            if (user.IsBlocked)
            {
                throw new UserBlockedException($"User with user name {user.Username} is blocked.");
            }

            comment.UserID = user.Id;
            comment.User = user;
            comment.PostID = post.Id;
            comment.Post = post;
            comment.CreateDate = DateTime.Now;

            if (baseCommentID != null)
            {
                Comment baseComment = this.commentsRepository.GetCommentById((int)baseCommentID);
                baseComment.Replies.Add(comment);
            }

            return this.commentsRepository.CreateComment(comment);
        }

        public Comment UpdateComment(User user, int id, string content)
        {
            var commentToUpdate = this.commentsRepository.GetCommentById(id);

            if (user.Id != commentToUpdate.UserID)
            {
                throw new AuthorizationException("Unauthorized");
            }

            return this.commentsRepository.UpdateComment(commentToUpdate, content);
        }

        public bool DeleteComment(User user, int id)
        {
            // TODO : check if we have to get the comment from the repo first
            // so we check if the user is the creator !?
            var commentToDelete = this.commentsRepository.GetCommentById(id);
            
            // IsDeleted check is done in the repo
            //if (commentToDelete.IsDeleted)
            //{
            //    throw new EntityNotFoundException($"Comment with id {id} not found");
            //}

            if (commentToDelete.UserID != user.Id && !user.IsAdmin)
            {
                throw new AuthorizationException($"Unauthorized.");
            }

            this.commentsRepository.DeleteComment(commentToDelete);

            return true;
        }
    }
}
