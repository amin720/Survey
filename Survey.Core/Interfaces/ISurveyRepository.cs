using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Survey.Core.Entities;

namespace Survey.Core.Interfaces
{
	public interface ISurveyRepository
	{
		Task<TBL_Surveys> Get(string name);
		Task<TBL_Surveys> Get(int id);
		Task<TBL_Surveys> Get();
		Task<IEnumerable<TBL_Surveys>> GetAll();
		Task Create(TBL_Surveys surveys);
		Task Edit(TBL_Surveys surveys,string olderSurvey);
		Task Delete(string name);
	}
}
