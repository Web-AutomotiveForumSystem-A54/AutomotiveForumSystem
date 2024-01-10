using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class PostsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
