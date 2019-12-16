using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Zac.Direct.Data.Model;
using Zac.Direct.Data.Repository;

namespace Zac.Direct.Data.SqlClient
{
    public abstract class BaseSqlSearchRepository<C,T,I> : BaseSqlRepository<T,I>, ISearchableRepository<T,C>
    {
		public BaseSqlSearchRepository(string connectionString) : base(connectionString) { }
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

		protected virtual void FillSearchCriteria(SqlCommand cmd, C crit)
		{
			FillSearchCriteria(cmd, crit, true);
		}

		protected virtual void FillSearchCriteria(SqlCommand cmd, C crit, bool cleanDefaults)
		{
			FillSearchCriteria(cmd, crit, cleanDefaults, false);
		}

		protected virtual void FillSearchCriteria(SqlCommand cmd, C crit, bool cleanDefaults, bool useStandardFilterPrefix)
		{
			FillSearchCriteria(cmd, crit, cleanDefaults, useStandardFilterPrefix, new List<string>());
		}

		protected virtual void FillSearchCriteria(SqlCommand cmd, C crit, bool cleanDefaults, bool useStandardFilterPrefix, List<string> excludedParams)
		{
			if (excludedParams == null)
			{
				excludedParams = new List<string>();
			}

			if (crit is ISearchCriteria)
			{
				var scrit = (ISearchCriteria)crit;
				if (scrit.RecordLimit.HasValue && !scrit.RecordStart.HasValue || !scrit.RecordLimit.HasValue && scrit.RecordStart.HasValue)
				{
					throw new Exception("Paging requires both RecordStart and RecordLimit");
				}
				else if (scrit.RecordLimit.HasValue || scrit.RecordStart.HasValue)
				{
					cmd.Parameters.AddWithValue("@RecordLimit", scrit.RecordLimit.Value);
					cmd.Parameters.AddWithValue("@RecordStart", scrit.RecordStart.Value);
				}
				//preference is to exclude parameters that are not in use
				excludedParams.Add("RecordLimit");
				excludedParams.Add("RecordStart");
			}

			TransformToParameters(cmd, crit, cleanDefaults, false, false, useStandardFilterPrefix ? "FilterFor" : "", excludedParams.ToArray());
		}

		protected virtual StandardTotals ParseTotals(SqlDataReader reader, StandardTotals knownTotals)
		{
			var result = knownTotals ?? new StandardTotals(Int64.MaxValue);
			if (result.TotalRecords == null)
			{
				result.TotalRecords = Convert.ToInt64(reader["TotalRows"]);
			}
			return result;
		}
	}
}
