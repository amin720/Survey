using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Survey.Core.Entities
{
	public class UserIdentity : IdentityUser
	{
		[StringLength(100)]
		public string DisplayName { get; set; }
		[StringLength(100)]
		public string FirstName { get; set; }
		[StringLength(100)]
		public string LastName { get; set; }
		[StringLength(12)]
		public string NationalCode { get; set; }
	}
}
