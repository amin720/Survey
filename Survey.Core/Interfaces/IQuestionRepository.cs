using System;
using Survey.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Survey.Core.Interfaces
{
	public interface IQuestionRepository
	{
		Task<TBL_Questions> Get(string title, int sectionId);
		Task<TBL_Questions> Get(int id, int sectionId);
		Task<List<TBL_Questions>> GetAllBySectionName(string sectionName);
		Task Create(TBL_Questions questions);
		Task Edit(TBL_Questions questions, string olderQuestion);
		Task Delete(string title, int sectionId);

		IQueryable<TBL_Questions> Query(Expression<Func<TBL_Questions, bool>> predicate);
	}
}
