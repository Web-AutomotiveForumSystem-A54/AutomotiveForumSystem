﻿using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.PostDtos;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace AutomotiveForumSystem.Controllers
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
                var posts = this.postService.GetAll(postQueryParameters);
                return Ok(posts);
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
                var posts = this.postService.GetPostsByUser(id, postQueryParameters);
                return Ok(posts);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/posts/postId
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(this.postService.GetPostById(id));
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST: api/posts
        [HttpPost]
        public IActionResult CreatePost([FromHeader]string credentials, [FromBody] PostModelCreate model)
        {
            try
            {
                var currentUser = this.authManager.TryGetUser(credentials);
                if (!this.ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var createdPost = this.postService.Create(this.postModelMapper.Map(model), currentUser);
                return StatusCode(StatusCodes.Status201Created, createdPost);
            }
            catch (BlockedUserException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (AuthenticationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/posts/{id}
        [HttpPut("{id}")]
        public IActionResult UpdatePost([FromHeader] string credentials, int id, [FromBody] Post post)
        {
            try
            {
                var currentUser = this.authManager.TryGetUser(credentials);
                var postToUpdate = this.postService.Update(id, post, currentUser);
                return Ok(postToUpdate);
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
        [HttpDelete("{id}")]
        public IActionResult DeletePost([FromHeader] string credentials, int id)
        {
            try
            {
                var currentUser = this.authManager.TryGetUser(credentials);
                this.postService.Delete(id, currentUser);
                return Ok();
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
