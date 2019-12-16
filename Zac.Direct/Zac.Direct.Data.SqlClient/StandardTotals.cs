using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct.Data.SqlClient
{
	public class StandardTotals
	{
		private readonly long RowsPerPage;
		public StandardTotals(long rowsPerPage)
		{
			RowsPerPage = rowsPerPage <= 0 ? Int64.MaxValue : rowsPerPage;
		}

		public long? TotalRecords { get; set; }
		public long TotalPages { get { return TotalRecords == null ? 0 : Math.Abs(TotalRecords.Value / RowsPerPage); } }
	}
}
