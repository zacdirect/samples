using System;

namespace Zac.Direct.Data.Repository
{
	/// <summary>
	/// Standardized CRUD and Error management
	/// </summary>
	/// <typeparam name="T">DataResultType</typeparam>
	/// <typeparam name="I">ID Type (Guid/Int/String)</typeparam>
	public interface IDataRepository<T, I> : IRaiseErrors
	{
		DataResult<T> Create(T obj);

		DataResult<T> Retrieve(I id);

		DataResult<T> Update(T obj);

		DataResult<bool> Delete(I id);
	}
}
