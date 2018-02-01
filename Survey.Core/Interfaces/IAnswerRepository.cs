using Survey.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Core.Interfaces
{
	public interface IAnswerRepository
	{
		Task<TBL_Answers> Get(string text, int questionId);
		Task<List<IGrouping<TBL_Questions, TBL_Answers>>> GetAll();
		Task Create(TBL_Answers answers);
		Task Edit(TBL_Answers answers);
		Task Delete(string text, int questionId);

	}
}
