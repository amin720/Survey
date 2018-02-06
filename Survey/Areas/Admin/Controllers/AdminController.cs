using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Survey.Areas.Admin.Security;
using Survey.Areas.Admin.ViewModels;
using Survey.Core.Entities;
using Survey.Core.Interfaces;
using Survey.Infrastructure.Repository;

namespace Survey.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	[RoutePrefix("")]
	[Authorize]
	public class AdminController : Controller
	{
		private readonly IUserRepository _userRepository;

		public AdminController()
			: this(new UserRepository())
		{
		}

		public AdminController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		// GET: Admin/Admin
		[Route("")]
		public async Task<ActionResult> Index()
		{
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;
			return View();
		}

		// GET: Admin/Admin/Login
		[HttpGet]
		[Route("login")]
		[AllowAnonymous]
		public async Task<ActionResult> Login()
		{
			return View();
		}

		// product: Admin/Admin/Login
		[HttpPost]
		[Route("login")]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		public virtual async Task<ActionResult> Login(LoginViewModel model)
		{
			try
			{
				//var response = Request["g-recaptcha-response"];
				//if (response != null && ReCaptcha.IsValid(response))
				//{


					var user = await _userRepository.GetLoginUserAsync(model.UserName, model.Password);

					if (user == null)
					{
						//ModelState.AddModelError(string.Empty, "The user with supplied credentials does not exist.");
						ModelState.AddModelError(string.Empty, "کاربر با مدارک ارائه شده موجود نیست.");
					}

					var authManager = HttpContext.GetOwinContext().Authentication;

					var userIdentity = await _userRepository.CreateIdentityAsync(user);

					authManager.SignIn(new AuthenticationProperties() { IsPersistent = model.Remmeberme }, userIdentity);


					return RedirectToAction("Index", "Admin");
				//}
				//return View();
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View(viewName: "Login");
			}
		}


		// GET: Admin/Admin/Logout
		[HttpGet]
		[Route("logout")]
		public async Task<ActionResult> Logout()
		{
			var authManager = HttpContext.GetOwinContext().Authentication;

			authManager.SignOut();

			return RedirectToAction("Index");
		}

		[AllowAnonymous]
		public async Task<PartialViewResult> AdminMenu()
		{
			var items = new List<AdminMenuItem>();

			if (User.Identity.IsAuthenticated)
			{
				items.Add(new AdminMenuItem
				{
					Text = "Admin Home",
					Action = "index",
					RouteInfo = new { controller = "admin", area = "admin" }
				});

				if (User.IsInRole("admin"))
				{
					items.Add(new AdminMenuItem
					{
						Text = "Users",
						Action = "index",
						RouteInfo = new { controller = "user", area = "admin" }
					});
				}
				else
				{
					items.Add(new AdminMenuItem
					{
						Text = "Profile",
						Action = "edit",
						RouteInfo = new { controller = "user", area = "admin", username = User.Identity.Name }
					});
				}

				if (!User.IsInRole("author"))
				{
					items.Add(new AdminMenuItem
					{
						Text = "Tags",
						Action = "index",
						RouteInfo = new { controller = "tag", area = "admin" }
					});
				}

				items.Add(new AdminMenuItem
				{
					Text = "Posts",
					Action = "index",
					RouteInfo = new { controller = "post", area = "admin" }
				});

				items.Add(new AdminMenuItem
				{
					Text = "Categories",
					Action = "index",
					RouteInfo = new { controller = "postcategory", area = "admin" }
				});
			}

			return PartialView(items);
		}

		[AllowAnonymous]
		public async Task<PartialViewResult> AuthenticationLink()
		{
			var item = new AdminMenuItem
			{
				RouteInfo = new { controller = "admin", area = "admin" }
			};

			if (User.Identity.IsAuthenticated)
			{
				item.Text = "Logout";
				item.Action = "logout";
			}
			else
			{
				item.Text = "Login";
				item.Action = "login";
			}

			return PartialView("_menuLink", item);
		}

		private bool _isDisposed;

		protected override void Dispose(bool disposing)
		{
			if (!_isDisposed)
			{
				_userRepository.Dispose();
			}

			_isDisposed = true;
			base.Dispose(disposing);
		}

		private async Task<UserIdentity> GetloggedInUser()
		{
			return await _userRepository.GetUserByNameAsync(User.Identity.Name);
		}

	}
}