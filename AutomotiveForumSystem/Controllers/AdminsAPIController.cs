using Microsoft.AspNetCore.Mvc;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    public class AdminsAPIController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAuthManager authManager;
        private readonly IUserMapper userMapper;

        public AdminsAPIController(IUsersService usersService, IAuthManager authManager, IUserMapper userMapper)
        {
            this.usersService = usersService;
            this.authManager = authManager;
            this.userMapper = userMapper;
        }

        [HttpGet("{username}")]
        public IActionResult GetByUsername([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] string username)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var user = this.usersService.GetByUsername(username);
                var response = this.userMapper.Map(user);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{email}")]
        public IActionResult GetByEmail([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] string email)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var user = this.usersService.GetByEmail(email);
                var response = this.userMapper.Map(user);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{firstName}")]
        public IActionResult GetByFirstName([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] string firstName)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var users = this.usersService.GetByFirstName(firstName);
                var response = this.userMapper.Map(users);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("block/{id}")]
        public IActionResult Block([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] int id)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var userToBlock = this.usersService.GetById(id);
                var blockedUser = this.usersService.Block(requestingUser, userToBlock);
                var response = this.userMapper.Map(blockedUser);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);                
            }
            catch (AuthorizationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UserBlockedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("unblock/{id}")]
        public IActionResult Unblock([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] int id)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var userToUnblock = this.usersService.GetById(id);
                var unblockedUser = this.usersService.Unblock(requestingUser, userToUnblock);
                var response = this.userMapper.Map(unblockedUser);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (AuthorizationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UserNotBlockedException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("setAdmin/{id}")]
        public IActionResult SetAdmin([FromHeader(Name = "Authorization")] string authorizationHeader, [FromRoute] int id)
        {
            try
            {
                var requestingUser = this.authManager.TryGetUserFromToken(authorizationHeader);
                var userToSetAsAdmin = this.usersService.GetById(id);
                var admin = this.usersService.SetAdmin(requestingUser, userToSetAsAdmin);
                var response = this.userMapper.Map(admin);

                return Ok(response);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (AuthorizationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UserNotBlockedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
