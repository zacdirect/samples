using System;
using System.Collections.Generic;
using System.Text;

namespace Zac.Direct.Tests.Model
{
	public class MockDataReturn
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public Guid? RelatedId { get; set; }
		public DateTime SampleTimeStamp { get; set; }
		public Guid SampleGuid { get; set; }
		
		public Guid[] SampleArray { get; set; }
		public List<Guid> SampleList { get; set; }
		
	}
}
