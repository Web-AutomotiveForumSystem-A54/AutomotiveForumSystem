using Microsoft.AspNetCore.Mvc;
using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Authorization;

namespace AutomotiveForumSystem.Controllers.Api
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
                var user = usersService.GetByUsername(username);
                var response = userMapper.Map(user);

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
                var user = usersService.GetByEmail(email);
                var response = userMapper.Map(user);

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
                var users = usersService.GetByFirstName(firstName);
                var response = userMapper.Map(users);

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
                var userToBlock = usersService.GetById(id);
                var blockedUser = usersService.Block(userToBlock);
                var response = userMapper.Map(blockedUser);

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
                var userToUnblock = usersService.GetById(id);
                var unblockedUser = usersService.Unblock(userToUnblock);
                var response = userMapper.Map(unblockedUser);

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
                var userToSetAsAdmin = usersService.GetById(id);
                var admin = usersService.SetAdmin(userToSetAsAdmin);
                var response = userMapper.Map(admin);

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
