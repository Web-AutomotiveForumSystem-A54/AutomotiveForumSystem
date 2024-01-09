using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AutomotiveForumSystem.Controllers.Api
{
    [Route("api/posts")]
    [ApiController]
    public class PostsAPIController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IPostModelMapper postModelMapper;
        private readonly IAuthManager authManager;

        public PostsAPIController(IPostService postService, IPostModelMapper postModelMapper, IAuthManager authManager)
        {
            this.postService = postService;
            this.postModelMapper = postModelMapper;
            this.authManager = authManager;
        }

        // GET: api/posts
        [HttpGet]
        public IActionResult Get([FromQuery] PostQueryParameters postQueryParameters)
        {
            try
            {
                var posts = postService.GetAll(postQueryParameters);
                return Ok(postModelMapper.MapPostsToResponseDtos(posts));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/posts/users/id
        [HttpGet("users/{id}")]
        public IActionResult GetPostsByUserId(int id, [FromQuery] PostQueryParameters postQueryParameters)
        {
            try
            {
                var posts = postService.GetPostsByUser(id, postQueryParameters);
                return Ok(postModelMapper.MapPostsToResponseDtos(posts));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/posts/id
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(postModelMapper.MapPostToResponseDto(postService.GetPostById(id)));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/posts
        [Authorize]
        [HttpPost]
        public IActionResult CreatePost([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] PostCreateDTO model)
        {
            try
            {
                var token = authorizationHeader.Replace("Bearer ", string.Empty);

                var currentUser = authManager.TryGetUserFromToken(token);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdPost = postService.CreatePost(postModelMapper.Map(model), currentUser);
                var postResponseDto = postModelMapper.MapPostToResponseDto(createdPost);
                return Ok(postResponseDto);
            }
            catch (UserBlockedException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/posts/id
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdatePost([FromHeader(Name = "Authorization")] string authorizationHeader, int id, [FromBody] PostCreateDTO model)
        {
            try
            {
                var token = authorizationHeader.Replace("Bearer ", string.Empty);

                var currentUser = authManager.TryGetUserFromToken(token);
                var postToUpdate = postService.Update(id, postModelMapper.Map(model), currentUser);
                return Ok(postModelMapper.MapPostToResponseDto(postToUpdate));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AuthorizationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/posts/id
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePost([FromHeader(Name = "Authorization")] string authorizationHeader, int id)
        {
            try
            {
                var token = authorizationHeader.Replace("Bearer ", string.Empty);

                var currentUser = authManager.TryGetUserFromToken(token);
                postService.DeletePost(id, currentUser);
                return Ok("Post deleted successfully!");
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (AuthorizationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
