using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Services.Contracts;

namespace AutomotiveForumSystem.Controllers.Api
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersAPIController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IUserMapper userMapper;
        private readonly IAuthManager authManager;

        public UsersAPIController(IUsersService usersService, IUserMapper userMapper, IAuthManager authManager)
        {
            this.usersService = usersService;
            this.userMapper = userMapper;
            this.authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create([FromBody] UserCreateDTO userDTO)
        {
            try
            {
                var user = userMapper.Map(userDTO);
                var createdUser = usersService.Create(user);
                var userResponse = userMapper.Map(createdUser);

                return StatusCode(StatusCodes.Status201Created, userResponse);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdateProfileInformation([FromHeader(Name = "Authorization")] string authorizationHeader, [FromBody] UserUpdateProfileInformationDTO userDTO)
        {
            try
            {
                var user = authManager.TryGetUserFromToken(authorizationHeader);
                var newUserInfo = userMapper.Map(userDTO);
                var updatedUser = usersService.UpdateProfileInformation(user.Id, newUserInfo);
                var userResponse = userMapper.Map(updatedUser);

                return Ok(userResponse);
            }
            catch (AuthenticationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (UserBlockedException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete]
        public IActionResult Delete([FromHeader(Name = "Authorization")] string authorizationHeader)
        {
            try
            {
                var user = authManager.TryGetUserFromToken(authorizationHeader);
                usersService.Delete(user);

                return Ok();
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
    }
}
