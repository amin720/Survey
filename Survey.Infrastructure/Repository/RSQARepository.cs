using Survey.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Survey.Core.Interfaces;

namespace Survey.Infrastructure.Repository
{
	public class RSQARepository : IRSQARepository
	{
		public async Task<TBL_RSQA> Get(int surveyId, int sectionId, int questionId, int answerId, int respondentId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_RSQA.SingleOrDefaultAsync(s => s.Survey_Id == surveyId && s.Section_Id == sectionId &&
																	s.Question_Id == questionId && s.Answer_Id == answerId &&
																	s.Respondent_Id == respondentId);
			}
		}
		public async Task<IEnumerable<TBL_RSQA>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_RSQA.ToListAsync();
			}
		}
		public async Task Create(TBL_RSQA rsqa)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(rsqa.Survey_Id, rsqa.Section_Id, rsqa.Question_Id, rsqa.Answer_Id, rsqa.Respondent_Id);

				if (model != null)
				{
					throw new KeyNotFoundException( "همچین مدلی وجود دارد.");
				}

				db.TBL_RSQA.Add(rsqa);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_RSQA rsqa)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(rsqa.Survey_Id, rsqa.Section_Id, rsqa.Question_Id, rsqa.Answer_Id, rsqa.Respondent_Id);

				if (model == null)
				{
					throw new KeyNotFoundException( "همچین مدلی وجود ندارد.");
				}

				model.Survey_Id = rsqa.Survey_Id;
				model.Section_Id= rsqa.Section_Id;
				model.Question_Id= rsqa.Question_Id;
				model.Answer_Id= rsqa.Answer_Id;
				model.Respondent_Id= rsqa.Respondent_Id;

				await db.SaveChangesAsync();
			}
		}
		public async Task Delete(int surveyId, int sectionId, int questionId, int answerId, int respondentId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(surveyId, sectionId, questionId, answerId, respondentId);

				if (model == null)
				{
					throw new KeyNotFoundException("همچین مدلی وجود ندارد.");
				}

				db.TBL_RSQA.Remove(model);
				await db.SaveChangesAsync();
			}
		}
	}
}
