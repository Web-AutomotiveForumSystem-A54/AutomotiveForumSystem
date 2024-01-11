using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthManager authManager;

		public AuthController(IAuthManager authManager)
		{
			this.authManager = authManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			var loginViewMode = new UserLoginViewModel();
			return View(loginViewMode);
		}

		[HttpPost]
		public IActionResult Login(UserLoginViewModel loginViewModel)
		{
			if (!ModelState.IsValid)
			{
				return View(loginViewModel);
			}

			try
			{
				var user = authManager.TryGetUser($"{loginViewModel.Username}:{loginViewModel.Password}");

				HttpContext.Session.SetString("CurrentUser", loginViewModel.Username);
				HttpContext.Session.SetString("IsUserlogged", "true");

				return RedirectToAction("Index", "Home");
			}
			catch (AuthenticationException ex)
			{
				ModelState.AddModelError("Username", ex.Message);
				ModelState.AddModelError("Password", ex.Message);
				return View(loginViewModel);
			}
		}
	}
}
