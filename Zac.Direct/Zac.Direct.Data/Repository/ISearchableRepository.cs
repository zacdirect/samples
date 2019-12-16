using Zac.Direct.Data.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct.Data.Repository
{
	/// <summary>
	/// Standardized Criteria Managament
	/// </summary>
	/// <typeparam name="T">DataResultType</typeparam>
	/// <typeparam name="C">SearchCriteria</typeparam>
	public interface ISearchableRepository<T,C> : IRaiseErrors
	{
		DataResult<T> Search(C crit);
	}
}
