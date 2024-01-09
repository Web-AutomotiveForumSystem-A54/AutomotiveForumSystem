using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.CommentDTOs;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers.Api
{
    [ApiController]
    [Route("api/comments")]
    public class CommentsAPIController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly ICommentsService commentsService;
        private readonly IAuthManager authManager;
        private readonly ICommentModelMapper commentModelMapper;

        public CommentsAPIController(IPostService postService,
            ICommentsService commentsService,
            IAuthManager authManager,
            ICommentModelMapper commentModelMapper)
        {
            this.postService = postService;
            this.commentsService = commentsService;
            this.authManager = authManager;
            this.commentModelMapper = commentModelMapper;
        }

        [HttpGet("")]
        public IActionResult GetAllComments([FromQuery] CommentQueryParameters commentQueryParameters)
        {
            try
            {
                var commentsToReturn = commentsService.GetAllComments(commentQueryParameters);
                return Ok(commentModelMapper.Map(commentsToReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCommentById(int id)
        {
            try
            {
                var commentToReturn = commentsService.GetCommentById(id);
                return Ok(commentModelMapper.Map(commentToReturn));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("replies/{id}")]
        public IActionResult GetAllRepliesByCommentId(int id)
        {
            try
            {
                var replies = commentsService.GetAllRepliesByCommentId(id);

                return Ok(commentModelMapper.Map(replies));
            }
            catch (Exception ex)
            {
                throw new EntityNotFoundException(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("")]
        public IActionResult CreateComment([FromHeader(Name = "Authorization")] string auth, [FromBody] CommentCreateDTO comment)
        {
            try
            {
                var token = auth.Replace("Bearer ", string.Empty);

                var user = authManager.TryGetUserFromToken(token);
                var createdComment = commentModelMapper.Map(comment);

                var post = postService.GetPostById(comment.PostID);

                commentsService.CreateComment(user, post, createdComment, comment.CommentID);

                return Ok(commentModelMapper.Map(createdComment));
            }
            catch (UserBlockedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (DuplicateEntityException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateComment([FromHeader(Name = "Authorization")] string auth, int id, [FromBody] CommentRequestDTO content)
        {
            try
            {
                Console.WriteLine(content);

                var token = auth.Replace("Bearer ", string.Empty);

                var user = authManager.TryGetUserFromToken(token);

                var updatedComment = commentsService.UpdateComment(user, id, content.Content);

                return Ok(commentModelMapper.Map(updatedComment));
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteComment([FromHeader(Name = "Authorization")] string auth, int id)
        {
            try
            {
                var user = authManager.TryGetUserFromToken(auth);
                var result = commentsService.DeleteComment(user, id);

                return Ok("Comment deleted.");
            }
            catch (AuthorizationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}