using Survey.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Survey.Core.Interfaces;

namespace Survey.Infrastructure.Repository
{
	public class QuestionRepository : IQuestionRepository
	{
		public async Task<TBL_Questions> Get(string title, int sectionId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Questions.SingleOrDefaultAsync(s => s.Title == title && s.Section_Id == sectionId);
			}
		}

		public async Task<List<IGrouping<TBL_Sections, TBL_Questions>>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Questions.GroupBy(q => q.TBL_Sections).ToListAsync();
			}
		}

		public async Task Create(TBL_Questions questions)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(questions.Title, questions.Section_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(questions.Title + "همچین مدلی وجود دارد."));
				}

				db.TBL_Questions.Add(questions);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Questions questions)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(questions.Title, questions.Section_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(questions.Title + "همچین مدلی وجود ندارد."));
				}

				model.Title = questions.Title;
				model.Description = questions.Description;
				model.TBL_RSQA = questions.TBL_RSQA;
				model.TBL_Sections = questions.TBL_Sections;
				model.ImageUrl = questions.ImageUrl;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string title, int sectionId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(title,sectionId);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(title + "همچین مدلی وجود ندارد."));
				}

				db.TBL_Questions.Remove(model);
				await db.SaveChangesAsync();
			}
		}

	}
}
