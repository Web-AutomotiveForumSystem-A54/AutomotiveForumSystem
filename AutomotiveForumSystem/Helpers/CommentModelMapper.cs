using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.CommentDTOs;
using AutomotiveForumSystem.Models.DTOs;

namespace AutomotiveForumSystem.Helpers
{
    public class CommentModelMapper : ICommentModelMapper
    {
        public CommentResponseDTO Map(Comment comment)
        {
            CommentResponseDTO commentDTO = new CommentResponseDTO();

            commentDTO.Post = comment.Post.Title;
            commentDTO.Content = comment.Content;
            commentDTO.User = comment.User.Username;
            commentDTO.CreatedDate = comment.CreateDate.ToString();
            commentDTO.Replies = new List<CommentResponseReplyDTO>();

            if (comment.Replies != null)
            {
                foreach (var item in comment.Replies)
                {
                    commentDTO.Replies.Add(new CommentResponseReplyDTO()
                    {
                        Content = item.Content,
                        User = item.User.Username,
                        CreatedDate = item.CreateDate.ToString(),
                    });
                }
            }

            return commentDTO;
        }

        public IList<CommentResponseDTO> Map(IList<Comment> comments)
        {
            List<CommentResponseDTO> commentResponseDTOs = new List<CommentResponseDTO>();

            foreach (var comment in comments)
            {
                commentResponseDTOs.Add(Map(comment));
            }

            return commentResponseDTOs;
        }

        public Comment Map(CommentCreateDTO comment)
        {
            var newComment = new Comment()
            {
                PostID = comment.PostID,
                Content = comment.Content,
            };

            return newComment;
        }
    }
}
