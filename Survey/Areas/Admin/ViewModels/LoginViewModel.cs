using System.ComponentModel.DataAnnotations;

namespace Survey.Areas.Admin.ViewModels
{
	public class LoginViewModel
	{
		[Required]
		[Display(Name = "Username")]
		public string UserName { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[RegularExpression(@"^.*(?=.*[!@#$%^&*\(\)_\-+=]).*$", ErrorMessage = "رمز عبور شما بایستی حروف a-z , A-z , @")]
		public string Password { get; set; }
		[DataType(DataType.EmailAddress)]
		[RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "ایمیل شما معتبر نیست")]
		public string Email { get; set; }

		[Display(Name = "Remmeber Me")]
		public bool Remmeberme { get; set; }
	}
}