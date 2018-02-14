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
	public class SectionRepository : ISectionRepository
	{
		public async Task<TBL_Sections> Get(string name, int surveyId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Sections.SingleOrDefaultAsync(s => s.Name == name && s.Survey_Id == surveyId);
			}
		}
		public async Task<TBL_Sections> Get(int id, int surveyId)
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Sections.SingleOrDefaultAsync(s => s.Id == id && s.Survey_Id == surveyId);
			}
		}

		public async Task<List<TBL_Sections>> GetAllBySurveyName(string surveyName)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Sections.Where(s => s.TBL_Surveys.Name == surveyName).ToListAsync();

				return model;
			}
		}

		public async Task Create(TBL_Sections sections)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Sections.SingleOrDefaultAsync(s => s.Name == sections.Name && s.Survey_Id == sections.Survey_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(sections.Name + "همچین مدلی وجود دارد.");
				}

				db.TBL_Sections.Add(sections);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Sections sections,string olderSection)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Sections.SingleOrDefaultAsync(s => s.Name == olderSection && s.Survey_Id == sections.Survey_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(sections.Name + "همچین مدلی وجود ندارد.");
				}

				model.Name = sections.Name;
				model.Description = sections.Description;
				model.Survey_Id = sections.Survey_Id;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string name, int surveyId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await db.TBL_Sections.SingleOrDefaultAsync(s => s.Name == name && s.Survey_Id == surveyId);

				if (model == null)
				{
					throw new KeyNotFoundException(name + "همچین مدلی وجود ندارد.");
				}

				db.TBL_Sections.Remove(model);
				await db.SaveChangesAsync();
			}
		}

		public IQueryable<TBL_Sections> Query(Expression<Func<TBL_Sections, bool>> predicate)
		{
			using (var db = new SurveyEntities())
			{
				return db.Set<TBL_Sections>().Where(predicate);
			}
		}
	}
}
