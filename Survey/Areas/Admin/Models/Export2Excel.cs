using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Survey.Areas.Admin.Models
{
	public class Export2Excel
	{
		public string Survey { get; set; }
		public string Section { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
		public string Gender { get; set; }
		public string Age { get; set; }
		public string Degree { get; set; }
	}
}