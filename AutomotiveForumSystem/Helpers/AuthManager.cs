using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Models;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;

namespace AutomotiveForumSystem.Helpers
{
    public class AuthManager : IAuthManager
    {
        private readonly IUsersService usersService;
        private readonly string jwtSecret;

        public AuthManager(IUsersService usersService, string jwtSecret)
        {
            this.usersService = usersService;
            this.jwtSecret = jwtSecret;
        }       

        public User TryGetUser(string credentials)
        {
            try
            {
                var credentialsArgs = credentials.Split(":");

                var username = credentialsArgs[0];
                var password = credentialsArgs[1];


                var user = this.usersService.GetByUsername(username);

                if (user.Password != password)
                {
                    throw new ApplicationException();
                }

                return user;
            }
            catch (ApplicationException)
            {
                throw new AuthenticationException("Invalid credentials");
            }
        }

        public User TryGetUserFromToken(string token)
        {
            token = token.Replace("Bearer", string.Empty);
            var username = GetUsernameFromToken(token);
            return usersService.GetByUsername(username);
        }

        private string GetUsernameFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            try
            {
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out securityToken);

                if (securityToken is JwtSecurityToken jwtSecurityToken &&
                    principal.Identity is ClaimsIdentity claimsIdentity)
                {
                    var usernameClaim = claimsIdentity.FindFirst(ClaimTypes.Name);
                    return usernameClaim?.Value;
                }
            }
            catch (SecurityTokenException)
            {
                throw new AuthenticationException("Invalid token");
            }

            return null;
        }

    }
}
