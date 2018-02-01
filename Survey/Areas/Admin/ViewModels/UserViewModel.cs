using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Survey.Core.Entities;

namespace Survey.Areas.Admin.ViewModels
{
	public class UserViewModel
	{
		[Required]
		[Display(Name = "Username")]
		public string UserName { get; set; }

		[Required]
		public string Email { get; set; }

		[Required]
		[Display(Name = "Display Name")]
		public string DisplayName { get; set; }

		[Display(Name = "Current Password")]
		public string CurrentPassword { get; set; }

		[Display(Name = "New Password")]
		[System.ComponentModel.DataAnnotations.Compare("ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match")]
		public string NewPassword { get; set; }

		[Display(Name = "Confirm Password")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Role")]
		[Required(ErrorMessage = "یک نقش را انتخاب کنید")]
		public string SelectedRole { get; set; }

		private readonly List<string> _roles = new List<string>();
		public IEnumerable<SelectListItem> Roles
		{
			get { return new SelectList(_roles); }
		}

		public IEnumerable<UserIdentity> Users { get; set; }

		public void LoadUserRoles(IEnumerable<IdentityRole> roles)
		{
			_roles.AddRange(roles.Select(r => r.Name));
		}



		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string NationalCode { get; set; }
		public string PhoneNumber { get; set; }
	}
}