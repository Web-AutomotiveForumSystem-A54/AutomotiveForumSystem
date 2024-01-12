using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AutomotiveForumSystem.Helpers
{
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
	public class IsAuthenticatedAttribute : Attribute, IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext context)
		{
			var isLogged = context.HttpContext.Session.Keys.Contains("CurrentUser");

			if (!isLogged)
				context.Result = new RedirectToRouteResult(new { controller = "Auth", action = "Login" });
		}
	}
}
