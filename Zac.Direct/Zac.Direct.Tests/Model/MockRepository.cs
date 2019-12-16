using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Zac.Direct.Data;
using Zac.Direct.Data.SqlClient;
using Zac.Direct.Tests.Model;

namespace Zac.Direct.Tests.Model
{
	public class MockRepository : BaseSqlSearchRepository<MockDataCriteria, MockDataReturn, int>
	{
		public MockRepository(string connectionString) : base(connectionString)
		{
			TableOfData = new MockDataReturn[] {
					new MockDataReturn() { Id = 1, Name = "First" },
					new MockDataReturn() { Id = 2, Name = "Second" },
					new MockDataReturn() { Id = 11, Name = "EleventyTwoith" },
					new MockDataReturn() { Id = 44, Name = "STRING" },
					new MockDataReturn() { Id = 56, Name = "UPDATED_STRING" },
					new MockDataReturn() { Id = 100, Name = "Hundred", RelatedId = Guid.Empty }
				}.ToList();
		}

		/// <summary>
		/// Exposed for test case purposes to know what data exists.  Simulates a SQL table.
		/// </summary>
		public List<MockDataReturn> TableOfData { get; set; }

		protected override DataResult<MockDataReturn> _AllImplementation()
		{
			return DataResultFactory<MockDataReturn>.Create(NewInstancesOf(TableOfData.ToArray()));
		}

		protected override DataResult<MockDataReturn> _CreateImplementation(MockDataReturn obj)
		{
			if (TableOfData.Where(m => m.Id == obj.Id).Count() > 0)
			{
				throw new Exception("Mock Constraint Error. " + obj.Id + " already exists in TableOfData");
			}

			var resultFromDataSource = new MockDataReturn()
			{
				Id = TableOfData.OrderByDescending(o => o.Id).First().Id + 1, //typically comes from sql and needs to be provided back to the client
				Name = obj.Name
			};
			TableOfData.Add(resultFromDataSource);
			return DataResultFactory<MockDataReturn>.Create(resultFromDataSource); //now whatever called this method knows the new id as well as any other defaults set along the business and repo layers
		}

		protected override DataResult<bool> _DeleteImplementation(int id)
		{
			TableOfData.RemoveAt(TableOfData.FindIndex(m => m.Id == id));

			return DataResultFactory<bool>.Create(true);
		}

		protected override DataResult<MockDataReturn> _RetrieveImplementation(int id)
		{
			return DataResultFactory<MockDataReturn>.Create(NewInstanceOf(TableOfData.First(m => m.Id == id)));
		}

		protected override DataResult<MockDataReturn> _SearchImplementation(MockDataCriteria crit)
		{
			//example of how it all comes together with real sql
			using (var conn = new SqlConnection(ConnectionString))
			{
				var cmd = new SqlCommand("mock_sproc", conn);
				FillSearchCriteria(cmd, crit);
				//conn.Open();
				//cmd.ExecuteNonQuery();
			}

			var results = TableOfData.ToList();
			if (crit.IdToOptionallySearch.HasValue)
			{
				results = TableOfData.Where(m => m.RelatedId.HasValue && m.RelatedId.Value == crit.IdToOptionallySearch).ToList();
			}

			//IsPaging could be used to optionally provide critera to a stored procedure
			long totalRecords = totalRecords = results.Count;
			if (crit.IsPaging())
			{
				results = results.Skip(crit.RecordStart.Value).Take(crit.RecordLimit.Value).ToList(); //stored proc would do this sort of thing
			}
			return DataResultFactory<MockDataReturn>.Create(results, totalRecords);

		}

		/// <summary>
		/// Example of how to take advantage of standard paging crit methods + adding whatever is special about this class type
		/// </summary>
		/// <param name="cmd"></param>
		/// <param name="crit"></param>
		protected override void FillSearchCriteria(SqlCommand cmd, MockDataCriteria crit)
		{
			base.FillSearchCriteria(cmd, crit);
			cmd.Parameters.AddWithValue("@IdToOptionallySearch", crit.IdToOptionallySearch);
		}

		protected override DataResult<MockDataReturn> _UpdateImplementation(MockDataReturn obj)
		{
			var match = TableOfData.FirstOrDefault(m => obj.Id == obj.Id);
			if (match == null)
			{
				throw new Exception("Mock Error. " + obj.Id + " not found in TableOfData");
			}

			match.Name = obj.Name == null ? null : new string(obj.Name.ToCharArray());
			return DataResultFactory<MockDataReturn>.Create(match);
		}

		/// <summary>
		/// Real repositories will be returning instances of some other type of data.  Be it a sql table, json store, whatever.  It will not be a memory instance of a locally stored list.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected MockDataReturn NewInstanceOf(MockDataReturn obj)
		{
			return new MockDataReturn() { Id = obj.Id, Name = obj.Name, RelatedId = obj.RelatedId };
		}

		/// <summary>
		/// Real repositories will be returning instances of some other type of data.  Be it a sql table, json store, whatever.  It will not be a memory instance of a locally stored list.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		protected MockDataReturn[] NewInstancesOf(MockDataReturn[] arr)
		{
			return arr.Select(obj => NewInstanceOf(obj)).ToArray();
		}

		public void AutoMapParameters(SqlCommand cmd, MockDataReturn obj)
		{
			TransformToParameters(cmd, obj);
		}

		public void AutoMapParameters(SqlCommand cmd, MockDataReturn obj, bool cleanDefaults)
		{
			TransformToParameters(cmd, obj, cleanDefaults, false);
		}

		public void AutoMapParameters(SqlCommand cmd, MockDataReturn obj, bool cleanDefaults, bool cleanZeroInts)
		{
			TransformToParameters(cmd, obj, cleanDefaults, cleanZeroInts, false);
		}

		public void AutoMapParameters(SqlCommand cmd, MockDataReturn obj, bool cleanDefaults, bool cleanZeroInts, bool detectIdAsOutput)
		{
			TransformToParameters(cmd, obj, cleanDefaults, cleanZeroInts, detectIdAsOutput);
		}

		public void AutoMapSearch(SqlCommand cmd, MockDataCriteria crit)
		{
			FillSearchCriteria(cmd, crit);
		}

		public void AutoMapSearch(SqlCommand cmd, MockDataCriteria crit, bool cleanDefaults)
		{
			FillSearchCriteria(cmd, crit, cleanDefaults);
		}

		public void AutoMapSearch(SqlCommand cmd, MockDataCriteria crit, bool cleanDefaults, bool useStandardTransforms)
		{
			FillSearchCriteria(cmd, crit, cleanDefaults, useStandardTransforms);
		}

		public void AutoMapSearch(SqlCommand cmd, MockDataCriteria crit, bool cleanDefaults, bool useStandardTransforms, List<string> excludedParams)
		{
			FillSearchCriteria(cmd, crit, cleanDefaults, useStandardTransforms, excludedParams);
		}
	}
}
