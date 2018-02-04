using Newtonsoft.Json;
using System.Collections.Generic;

namespace Survey.Areas.Admin.Security
{
	public class RepaptchaResponse
	{
		[JsonProperty("success")]
		public bool Success { get; set; }

		[JsonProperty("error-codes")]
		public List<string> ErrorCodes { get; set; }
	}
}