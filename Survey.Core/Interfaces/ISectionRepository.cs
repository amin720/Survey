using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Survey.Core.Entities;

namespace Survey.Core.Interfaces
{
	public interface ISectionRepository
	{
		Task<TBL_Sections> Get(string name, int surveyId);
		Task<List<IGrouping<TBL_Surveys, TBL_Sections>>> GetAll();
		Task Create(TBL_Sections sections);
		Task Edit(TBL_Sections sections);
		Task Delete(string name, int surveyId);
	}
}
