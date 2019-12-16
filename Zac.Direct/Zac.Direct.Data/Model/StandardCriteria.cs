using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct.Data.Model
{
    public class StandardCriteria : ISearchCriteria
    {
        public string OrderBy { get; set; }
        public int? RecordStart { get; set; }
        public int? RecordLimit { get; set; }
        public bool IsPaging()
        {
			if(RecordLimit.HasValue && !RecordStart.HasValue || !RecordLimit.HasValue && RecordStart.HasValue)
			{
				throw new Exception("Paging requires both RecordStart and RecordLimit");
			}
			else if (RecordLimit.HasValue)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
    }
}
