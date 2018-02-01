using System.Collections.Generic;
using System.Threading.Tasks;
using Survey.Core.Entities;

namespace Survey.Core.Interfaces
{
	public interface IRespondentRepository
	{
		Task<TBL_Respondents> Get(string email);
		Task<IEnumerable<TBL_Respondents>> GetAll();
		Task Create(TBL_Respondents respondents);
		Task Edit(TBL_Respondents respondents);
		Task Delete(string email);

	}
}
