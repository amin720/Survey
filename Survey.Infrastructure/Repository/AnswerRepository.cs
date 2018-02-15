using Survey.Core.Entities;
using Survey.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Repository
{
	public class AnswerRepository : IAnswerRepository
	{
		public async Task<TBL_Answers> Get(string text, int questionId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Answers.SingleOrDefaultAsync(s => s.Text == text && s.Question_Id == questionId);
			}
		}
		public async Task<TBL_Answers> Get(int id, int questionId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Answers.SingleOrDefaultAsync(s => s.Id == id && s.Question_Id == questionId);
			}
		}

		public async Task<List<TBL_Answers>> GetAllByQuestionName(string questionName)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Answers.Where(s => s.TBL_Questions.Title== questionName).ToListAsync();

				return model;
			}
		}

		public async Task Create(TBL_Answers answers)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Answers.SingleOrDefaultAsync(s => s.Id == answers.Id && s.Question_Id == answers.Question_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(answers.Text + "همچین مدلی وجود دارد.");
				}

				db.TBL_Answers.Add(answers);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Answers answers,string olderText)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Answers.SingleOrDefaultAsync(s => s.Text == olderText && s.Question_Id == answers.Question_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(answers.Text + "همچین مدلی وجود ندارد.");
				}

				model.Text = answers.Text;
				model.Question_Id = answers.Question_Id;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string text, int questionId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Answers.SingleOrDefaultAsync(s => s.Text == text && s.Question_Id == questionId);

				if (model == null)
				{
					throw new KeyNotFoundException(text + "همچین مدلی وجود ندارد.");
				}

				db.TBL_Answers.Remove(model);
				await db.SaveChangesAsync();
			}
		}

		public IQueryable<TBL_Answers> Query(Expression<Func<TBL_Answers, bool>> predicate)
		{
			using (var db = new SurveyEntities())
			{
				return db.Set<TBL_Answers>().Where(predicate);
			}
		}
	}
}
