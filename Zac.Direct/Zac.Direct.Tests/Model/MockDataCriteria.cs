using System;
using Zac.Direct.Data.Model;

namespace Zac.Direct.Tests.Model
{
	public class MockDataCriteria : StandardCriteria
	{
		public Guid? IdToOptionallySearch { get; set; }
		public Guid SillyParameterDefault { get; set; }
		public int SillyIntDefault { get; set; }
		
	}
}
