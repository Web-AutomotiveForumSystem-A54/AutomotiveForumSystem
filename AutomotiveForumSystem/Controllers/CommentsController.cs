using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class CommentsController : Controller
	{
		private readonly IUsersService usersService;
		private readonly ICommentsService commentsService;
		private readonly IAuthManager authManager;

		public CommentsController(IUsersService usersService, ICommentsService commentsService, IAuthManager authManager)
		{
			this.usersService = usersService;
			this.commentsService = commentsService;
			this.authManager = authManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Delete([FromRoute] int commentId, PostDataViewModel postModel)
		{
			// TODO : check if user is logged in

			if (!HttpContext.Session.Keys.Contains("CurrentUser"))
			{
				return RedirectToAction("Login", "Auth");
			}

			try
			{
				var userName = HttpContext.Session.GetString("CurrentUser");
				var user = usersService.GetByUsername(userName);
				var comment = commentsService.DeleteComment(user, commentId);

				return RedirectToAction("Index", "Home", new { Id = postModel.Id });
			}
			catch (Exception ex)
			{
				return RedirectToAction("Index", "Home");
			}
		}
	}
}
