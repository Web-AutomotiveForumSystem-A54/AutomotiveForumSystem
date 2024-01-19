using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Models.DTOs;
using AutomotiveForumSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Security.Cryptography;
using System.Text;

namespace AutomotiveForumSystem.Controllers.Api
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthManager authManager;
        private readonly JwtService jwtService;
        public AuthAPIController(IAuthManager authManager, JwtService jwtService)
        {
            this.authManager = authManager;
            this.jwtService = jwtService;
        }
        [HttpPost("login")]
        public IActionResult Login([FromHeader] string credentials)
        {
            var user = authManager.TryGetUser(credentials);
            var token = jwtService.GenerateToken(user.Username, user.IsAdmin);
            return Ok(new { Token = token });
        }
    }
}
