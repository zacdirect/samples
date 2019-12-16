using System;
using Zac.Direct.Data.Model;

namespace Zac.Direct.Data.Repository
{
	/// <summary>
	/// Standardized CRUD, Criteria, and Error management
	/// </summary>
	/// <typeparam name="T">DataResultType</typeparam>
	/// <typeparam name="C">SearchCriteria</typeparam>
	/// <typeparam name="I">ID Type (Guid/Int/String)</typeparam>
	public abstract class BaseSearchRepository<T, C, I> : BaseRepository<T, I>, ISearchableRepository<T, C>
	{
		protected abstract DataResult<T> _SearchImplementation(C crit);

		public DataResult<T> Search(C crit)
		{
			try
			{
				return _SearchImplementation(crit);
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<T>.Create(ex);
			}
		}
	}
}
