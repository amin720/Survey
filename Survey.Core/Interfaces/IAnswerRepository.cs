using Survey.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Core.Interfaces
{
	public interface IAnswerRepository
	{
		Task<TBL_Answers> Get(string text, int questionId);
		Task<TBL_Answers> Get(int id, int questionId);
		Task<List<TBL_Answers>> GetAllByQuestionName(string questionName);
		Task Create(TBL_Answers answers);
		Task Edit(TBL_Answers answers, string olderText);
		Task Delete(string text, int questionId);

	}
}
