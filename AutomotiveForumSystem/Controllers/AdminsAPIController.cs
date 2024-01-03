using Microsoft.AspNetCore.Mvc;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace AutomotiveForumSystem.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    [ApiController]
    [Route("api/admin/users")]
    public class AdminsAPIController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IUserMapper userMapper;

        public AdminsAPIController(IUsersService usersService, IUserMapper userMapper)
        {
            this.usersService = usersService;
            this.userMapper = userMapper;
        }

        [HttpGet("username/{username}")]
        public IActionResult GetByUsername([FromRoute] string username)
        {
            try
            {
                var user = this.usersService.GetByUsername(username);
                var response = this.userMapper.Map(user);

                return Ok(response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("email/{email}")]
        public IActionResult GetByEmail([FromRoute] string email)
        {
            try
            {
                var user = this.usersService.GetByEmail(email);
                var response = this.userMapper.Map(user);

                return Ok(response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("firstName/{firstName}")]
        public IActionResult GetByFirstName([FromRoute] string firstName)
        {
            try
            {
                var users = this.usersService.GetByFirstName(firstName);
                var response = this.userMapper.Map(users);

                return Ok(response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPut("block/{id}")]
        public IActionResult Block([FromRoute] int id)
        {
            try
            {
                var userToBlock = this.usersService.GetById(id);
                var blockedUser = this.usersService.Block(userToBlock);
                var response = this.userMapper.Map(blockedUser);

                return Ok(response);
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
        public IActionResult Unblock([FromRoute] int id)
        {
            try
            {
                var userToUnblock = this.usersService.GetById(id);
                var unblockedUser = this.usersService.Unblock(userToUnblock);
                var response = this.userMapper.Map(unblockedUser);

                return Ok(response);
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
        public IActionResult SetAdmin([FromRoute] int id)
        {
            try
            {
                var userToSetAsAdmin = this.usersService.GetById(id);
                var admin = this.usersService.SetAdmin(userToSetAsAdmin);
                var response = this.userMapper.Map(admin);

                return Ok(response);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (UserNotBlockedException e)
            {
                return BadRequest(e.Message);
            }
            catch (AdminRightsAlreadyGrantedException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
