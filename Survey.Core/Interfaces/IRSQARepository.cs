using System.Collections.Generic;
using System.Threading.Tasks;
using Survey.Core.Entities;

namespace Survey.Core.Interfaces
{
	public interface IRSQARepository
	{
		Task<TBL_RSQA> Get(int surveyId, int sectionId, int questionId, int answerId, int respondentId);
		Task<IEnumerable<TBL_RSQA>> GetAll();
		Task Create(TBL_RSQA rsqa);
		Task Edit(TBL_RSQA rsqa);
		Task Delete(int surveyId, int sectionId, int questionId, int answerId, int respondentId);
	}
}
