using Survey.Core.Entities;
using Survey.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

		public async Task<List<IGrouping<TBL_Questions, TBL_Answers>>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Answers.GroupBy(q => q.TBL_Questions).ToListAsync();
			}
		}

		public async Task Create(TBL_Answers answers)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(answers.Text, answers.Question_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(answers.Text + "همچین مدلی وجود دارد."));
				}

				db.TBL_Answers.Add(answers);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Answers answers)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(answers.Text, answers.Question_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(answers.Text + "همچین مدلی وجود ندارد."));
				}

				model.Text = answers.Text;
				model.TBL_RSQA = answers.TBL_RSQA;
				model.Question_Id = answers.Question_Id;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string text, int questionId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(text, questionId);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(text + "همچین مدلی وجود ندارد."));
				}

				db.TBL_Answers.Remove(model);
				await db.SaveChangesAsync();
			}
		}
	}
}
