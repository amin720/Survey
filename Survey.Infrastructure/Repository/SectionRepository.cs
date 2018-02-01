using Survey.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

		public async Task<List<IGrouping<TBL_Surveys, TBL_Sections>>> GetAll()
		{
			using (var db = new SurveyEntities())
			{
				return await db.TBL_Sections.GroupBy(q => q.TBL_Surveys).ToListAsync();
			}
		}

		public async Task Create(TBL_Sections sections)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(sections.Name, sections.Survey_Id);

				if (model != null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(sections.Name + "همچین مدلی وجود دارد."));
				}

				db.TBL_Sections.Add(sections);
				await db.SaveChangesAsync();
			}
		}
		public async Task Edit(TBL_Sections sections)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(sections.Name, sections.Survey_Id);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(sections.Name + "همچین مدلی وجود ندارد."));
				}

				model.Name = sections.Name;
				model.TBL_RSQA = sections.TBL_RSQA;
				model.Survey_Id = sections.Survey_Id;

				await db.SaveChangesAsync();
			}
		}

		public async Task Delete(string name, int surveyId)
		{
			using (var db = new SurveyEntities())
			{
				var model = await Get(name, surveyId);

				if (model == null)
				{
					throw new KeyNotFoundException(String.Empty, new Exception(name + "همچین مدلی وجود ندارد."));
				}

				db.TBL_Sections.Remove(model);
				await db.SaveChangesAsync();
			}
		}
	}
}
