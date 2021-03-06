﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Survey.Core.Entities;

namespace Survey.ViewModels
{
	public class ExamViewModel
	{
		public int Section_Id { get; set; }
		public string QuestionTitle { get; set; }
		public string QuestionDescription { get; set; }
		public string QuestionImgeUrl { get; set; }
		public string Answer { get; set; }
		public string StartLabel { get; set; }
		public string EndLabel { get; set; }

		public string Section { get; set; }
		public string Survey { get; set; }
		public string Email { get; set; }
		public int Index { get; set; }

		public IList<TBL_Sections> Sections { get; set; }
	}
}