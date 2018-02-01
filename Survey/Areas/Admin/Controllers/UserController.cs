using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Survey.Areas.Admin.Services;
using Survey.Areas.Admin.ViewModels;
using Survey.Core.Entities;
using Survey.Core.Interfaces;
using Survey.Infrastructure.Repository;

namespace Survey.Areas.Admin.Controllers
{
	[RouteArea("Admin")]
	[RoutePrefix("User")]
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUserRepository _userRepository;
		private readonly IRoleRepository _roleRepository;
		private readonly UserService _users;

		public UserController()
		{
			_userRepository = new UserRepository();
			_roleRepository = new RoleRepository();
			_users = new UserService(ModelState, _userRepository, _roleRepository);
		}

		// GET: Admin/User
		[Route("")]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Index()
		{
			var model = await _userRepository.GetAllUsersAsync();
			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}

		// GET: Admin/User/Create
		[Route("Create")]
		[HttpGet]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Create()
		{
			var model = new UserViewModel();
			model.LoadUserRoles(await _roleRepository.GetAllRolesAsync());

			var user = await GetloggedInUser();
			ViewBag.Username = user.UserName;
			ViewBag.DisplayName = user.DisplayName;

			return View(model: model);
		}

		// product: Admin/User/Create
		[Route("Create")]
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Create(UserViewModel model)
		{
			try
			{
				var completed = await _users.CreateAsync(model);

				if (completed)
				{
					return RedirectToAction("Index");
				}

				var user = await GetloggedInUser();
				ViewBag.Username = user.UserName;
				ViewBag.DisplayName = user.DisplayName;

				return View(model: model);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View();
			}

		}

		// GET: Admin/User/Edit/username
		[HttpGet]
		[Route("Edit/{username}")]
		[Authorize(Roles = "admin, editor, author")]
		public async Task<ActionResult> Edit(string username)
		{
			var currentUser = User.Identity.Name;

			if (!User.IsInRole("admin") &&
				!string.Equals(currentUser, username, StringComparison.CurrentCultureIgnoreCase))
			{
				return new HttpUnauthorizedResult();
			}

			var user = await _users.GetUserByNameAsync(username);

			if (user == null)
			{
				return HttpNotFound();
			}

			var userC = await GetloggedInUser();
			ViewBag.Username = userC.UserName;
			ViewBag.DisplayName = userC.DisplayName;

			return View(model: user);
		}

		// product: Admin/User/Edit/username
		[HttpPost]
		[Route("Edit")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin, editor, author")]
		public async Task<ActionResult> Edit(UserViewModel model)
		{
			try
			{
				var currentUser = User.Identity.Name;
				var isAdmin = User.IsInRole("admin");

				if (!isAdmin &&
				    !string.Equals(currentUser, model.UserName, StringComparison.CurrentCultureIgnoreCase))
				{
					return new HttpUnauthorizedResult();
				}

				var userUpdated = await _users.UpdateUser(model);

				if (userUpdated)
				{
					if (isAdmin)
					{
						return RedirectToAction("index");
					}

					return RedirectToAction("index", "Admin");
				}

				var user = await GetloggedInUser();
				ViewBag.Username = user.UserName;
				ViewBag.DisplayName = user.DisplayName;

				return View(model: model);
			}
			catch (Exception e)
			{
				ModelState.AddModelError(string.Empty, e.Message);
				return View();
			}
		}

		// /admin/User/delete/product-to-delete
		[HttpGet]
		[Route("Delete/{username}")]
		[Authorize(Roles = "admin, editor")]
		[AllowAnonymous]
		public async Task<ActionResult> Delete(string username)
		{
			var user = await _userRepository.GetUserByNameAsync(username);

			if (user == null)
			{
				return HttpNotFound();
			}

			var userC = await GetloggedInUser();
			ViewBag.Username = userC.UserName;
			ViewBag.DisplayName = userC.DisplayName;

			return View(user);
		}

		// product: Admin/User/Delete
		[HttpPost]
		[Route("Delete/{username}")]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "admin")]
		public async Task<ActionResult> Delete(string username, int? foo)
		{
			await _users.DeleteAsync(username);
			return RedirectToAction("Index", "User");
		}

		private bool _isDisposed;
		protected override void Dispose(bool disposing)
		{

			if (!_isDisposed)
			{
				_userRepository.Dispose();
				_roleRepository.Dispose();
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