using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class AuthController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
            return View();
		}
	}
}
