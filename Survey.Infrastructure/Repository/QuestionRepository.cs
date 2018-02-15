using Survey.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
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
		public async Task<TBL_Questions> Get(int id, int sectionId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Questions.SingleOrDefaultAsync(s => s.Id == id && s.Section_Id == sectionId);
			}
		}

		public async Task<List<TBL_Questions>> GetAllBySectionName(string sectionName)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Questions.Where(s => s.TBL_Sections.Name == sectionName).ToListAsync();

				return model;
			}
		}

		public IQueryable<TBL_Questions> Query(Expression<Func<TBL_Questions, bool>> predicate)
		{
			using (var db = new SurveyEntities())
			{
				return db.Set<TBL_Questions>().Where(predicate);
			}
		}

		public async Task Create(TBL_Questions questions)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Questions.SingleOrDefaultAsync(s => s.Id == questions.Id && s.Section_Id == questions.Section_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(questions.Title + "همچین مدلی وجود دارد.");
				}

				db.TBL_Questions.Add(questions);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Questions questions,string olderQuestion)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Questions.SingleOrDefaultAsync(s => s.Id == questions.Id && s.Section_Id == questions.Section_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(questions.Title + "همچین مدلی وجود ندارد.");
				}

				model.Title = questions.Title;
				model.Description = questions.Description;
				model.Section_Id = questions.Section_Id;
				model.ImageUrl = questions.ImageUrl;
				model.StartLabel = questions.StartLabel;
				model.EndLabel = questions.EndLabel;


				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string title, int sectionId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Questions.SingleOrDefaultAsync(s => s.Title == title && s.Section_Id == sectionId);

				if (model == null)
				{
					throw new KeyNotFoundException(title + "همچین مدلی وجود ندارد.");
				}

				db.TBL_Questions.Remove(model);
				await db.SaveChangesAsync();
			}
		}

	}
}
