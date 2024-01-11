using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class LoginController : Controller
	{
		[HttpGet]
		public IActionResult Login()
		{
            return View();
		}
	}
}
