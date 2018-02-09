using Survey.Core.Entities;
using Survey.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Repository
{
	public class RespondentRepository : IRespondentRepository
	{
		public async Task<TBL_Respondents> Get(string email)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Respondents.SingleOrDefaultAsync(s => s.Email == email);
			}
		}

		public async Task<IEnumerable<TBL_Respondents>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Respondents.OrderBy(s => s.Email).ToListAsync();
			}
		}

		public async Task Create(TBL_Respondents respondents)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(respondents.Email);

				if (model != null)
				{
					throw new KeyNotFoundException(respondents.Email + " همچین کاربری ثبت کرده است . ");
				}

				db.TBL_Respondents.Add(respondents);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Respondents respondents)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(respondents.Email);

				if (model == null)
				{
					throw new KeyNotFoundException(respondents.Email + "همچین مدلی وجود ندارد.");
				}

				model.Email = respondents.Email;
				model.Age = respondents.Age;
				model.TBL_RSQA = respondents.TBL_RSQA;
				model.Degree = respondents.Degree;
				model.DurationMarried = respondents.DurationMarried;
				model.Gender = respondents.Gender;
				model.Major = respondents.Major;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string email)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(email);

				if (model == null)
				{
					throw new KeyNotFoundException(email + "همچین مدلی وجود ندارد.");
				}

				db.TBL_Respondents.Remove(model);
				await db.SaveChangesAsync();
			}
		}
	}
}
