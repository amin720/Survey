using Survey.Core.Entities;
using Survey.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Repository
{
	public class SurveyRepository : ISurveyRepository
	{
		public async Task<TBL_Surveys> Get(string name)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Surveys.SingleOrDefaultAsync(s => s.Name == name);
			}
		}
		public async Task<TBL_Surveys> Get(int id)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Surveys.SingleOrDefaultAsync(s => s.Id == id);
			}
		}
		public async Task<IEnumerable<TBL_Surveys>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Surveys.OrderBy(s => s.Name).ToListAsync();
			}
		}
		public async Task Create(TBL_Surveys surveys)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(surveys.Name);

				if (model != null)
				{
					throw new KeyNotFoundException(surveys.Name + "همچین مدلی وجود دارد.");
				}

				db.TBL_Surveys.Add(surveys);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Surveys surveys, string olderSurvey)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(olderSurvey);

				if (model == null)
				{
					throw new KeyNotFoundException(olderSurvey + "همچین مدلی وجود ندارد.");
				}

				model.Name = surveys.Name;
				model.Description = surveys.Description;
				model.User_Id = surveys.User_Id;

				await db.SaveChangesAsync();
			}
		}
		public async Task Delete(string name)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(name);

				if (model == null)
				{
					throw new KeyNotFoundException(name + "همچین مدلی وجود ندارد.");
				}

				db.TBL_Surveys.Remove(model);
				await db.SaveChangesAsync();
			}
		}
	}
}
