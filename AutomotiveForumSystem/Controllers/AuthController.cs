using AutomotiveForumSystem.Exceptions;
using AutomotiveForumSystem.Helpers;
using AutomotiveForumSystem.Helpers.Contracts;
using AutomotiveForumSystem.Models.ViewModels;
using AutomotiveForumSystem.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AutomotiveForumSystem.Controllers
{
	public class AuthController : Controller
	{
		private readonly IAuthManager authManager;
		private readonly IUsersService usersService;
		private readonly IUserMapper userMapper;

		public AuthController(IAuthManager authManager, IUsersService usersService, IUserMapper userMapper)
		{
			this.authManager = authManager;
			this.usersService = usersService;
			this.userMapper = userMapper;
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

				return RedirectToAction("Index", "Home");
			}
			catch (AuthenticationException ex)
			{
				ModelState.AddModelError("Username", ex.Message);
				ModelState.AddModelError("Password", ex.Message);
				return View(loginViewModel);
			}
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			var registerViewModel = new UserRegisterViewModel();
			return View(registerViewModel);
		}

		[HttpPost]
		public IActionResult Register(UserRegisterViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			try
			{
				var user = this.userMapper.Map(model);
				_ = this.usersService.Create(user);

				HttpContext.Session.SetString("isRegistered", "true");

				return RedirectToAction("Login", "Auth");
			}
			catch (DuplicateEntityException ex)
			{
				// TODO: Ask if this is okay
				if (ex.Message.Contains("Email"))
				{
					ModelState.AddModelError("Email", ex.Message);
				}
				else
				{
					ModelState.AddModelError("Username", ex.Message);
				}

				return View(model);				
			}
		}
	}
}
