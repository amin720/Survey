using Survey.Core.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Survey.Core.Interfaces
{
	public interface IQuestionRepository
	{
		Task<TBL_Questions> Get(string title, int sectionId);
		Task<List<IGrouping<TBL_Sections, TBL_Questions>>> GetAll();
		Task Create(TBL_Questions questions);
		Task Edit(TBL_Questions questions);
		Task Delete(string title, int sectionId);
	}
}
