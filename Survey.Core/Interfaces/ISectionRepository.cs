using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Survey.Core.Entities;

namespace Survey.Core.Interfaces
{
	public interface ISectionRepository
	{
		Task<TBL_Sections> Get(string name, int surveyId);
		Task<TBL_Sections> Get(int id, int surveyId);
		Task<List<TBL_Sections>> GetAllBySurveyName(string surveyName);
		Task Create(TBL_Sections sections);
		Task Edit(TBL_Sections sections);
		Task Delete(string name, int surveyId);
		IQueryable<TBL_Sections> Query(Expression<Func<TBL_Sections, bool>> predicate);
	}
}
