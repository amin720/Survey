using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Survey.Core.Entities;

namespace Survey.Areas.Admin.ViewModels
{
	public class SurveyViewModel
	{
		public string SurveyTitle { get; set; }
		public string SurveyDescription { get; set; }

		public string SectionTitle { get; set; }
		public string SectionDescription { get; set; }

		public string QuestionTitle { get; set; }
		public string QuestionDescription { get; set; }
		public string QuestionImageUrl { get; set; }

		public string Option { get; set; }	

		public IEnumerable<TBL_Surveys> Surveyses { get; set; }
		public List<TBL_Sections> Sectionses { get; set; }
		public List<TBL_Questions> Questions { get; set; }
		public List<TBL_Answers> Answerses { get; set; }
	}
}