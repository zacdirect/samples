using System;

namespace Zac.Direct.Data.Repository
{
	public abstract class BaseRepository<T, I> : IDataRepository<T, I>
	{
		public event EventHandler<ErrorEventArgs> OnError;
		public void RaiseErrorEvent(Exception ex)
		{
			RaiseErrorEvent(new ErrorEventArgs(ex));
		}

		public void RaiseRepositoryError(string message)
		{
			RaiseErrorEvent(new ErrorEventArgs(message));
		}

		public void RaiseErrorEvent(ErrorEventArgs args)
		{
			OnError?.Invoke(this, args);
		}

		protected abstract DataResult<T> _CreateImplementation(T obj);
		protected abstract DataResult<T> _RetrieveImplementation(I id);
		protected abstract DataResult<T> _UpdateImplementation(T obj);
		protected abstract DataResult<bool> _DeleteImplementation(I id);

		protected abstract DataResult<T> _AllImplementation();

		public DataResult<T> Create(T obj)
		{
			try
			{
				return _CreateImplementation(obj);
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<T>.Create(ex);
			}
		}

		public DataResult<bool> Delete(I id)
		{
			try
			{
				return _DeleteImplementation(id);
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<bool>.Create(ex);
			}
		}

		public DataResult<T> Retrieve(I id)
		{
			try
			{
				return _RetrieveImplementation(id);
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<T>.Create(ex);
			}
		}

		public DataResult<T> Update(T obj)
		{
			try
			{
				return _UpdateImplementation(obj);
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<T>.Create(ex);
			}
		}

		public DataResult<T> All()
		{
			try
			{
				return _AllImplementation();
			}
			catch (Exception ex)
			{
				RaiseErrorEvent(ex);
				return DataResultFactory<T>.Create(ex);
			}
		}
	}
}
