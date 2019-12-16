using System;
using System.Data;
using System.Data.SqlClient;
using NUnit.Framework;
using Zac.Direct.Tests.Model;

namespace Zac.Direct.Tests
{
    public class DataTests
    {
        [SetUp]
        public void Setup()
        {
        }

		[Test]
		public void ParameterlessSearchWorks()
		{
			var repo = new MockRepository("");
			Assert.AreEqual(repo.TableOfData.Count, repo.Search(new MockDataCriteria()).ResultCollection.Count);
		}

		[Test]
		public void ParameterSearchWorks()
		{
			var repo = new MockRepository("");
			Assert.AreEqual(1, repo.Search(new MockDataCriteria() { IdToOptionallySearch = Guid.Empty }).ResultCollection.Count);
		}

		[Test]
		public void PagingSanityChecksWork()
		{
			var repo = new MockRepository("");
			var badCrit = new MockDataCriteria() { RecordStart = 1 };
			var badCrit2 = new MockDataCriteria() { RecordLimit = 1 };
			Assert.IsFalse(repo.Search(badCrit).IsSuccessful);
			Assert.IsFalse(repo.Search(badCrit2).IsSuccessful);
		}

		[Test]
		public void ErrorEventRaised()
		{
			var repo = new MockRepository("");
			var monitor = new ErrorEventMonitor();
			Assert.IsFalse(monitor.IsErrorRaised);
			monitor.Monitor(repo);
			repo.Retrieve(-1);
			Assert.IsTrue(monitor.IsErrorRaised);
		}

		[Test]
		public void TransformationMethodsWork()
		{
			var repo = new MockRepository("");
			var goodCrit = new MockDataCriteria();
			var badCrit = new MockDataCriteria() { RecordStart = 1 };
			Assert.IsNotNull(repo.Search(goodCrit).ToSuccessfulList());
			Assert.Throws<Exception>(() => repo.Search(badCrit).ToSuccessfulList());
		}

		[Test]
		public void PagingWorks()
		{
			var repo = new MockRepository("");
			var goodCrit = new MockDataCriteria() { RecordStart = 1, RecordLimit = 2 };

			var wrapper = repo.Search(goodCrit);
			var result = wrapper.ToSuccessfulList();
			Assert.IsTrue(result.Count == 2);
			Assert.IsTrue(wrapper.RowCount == 6);
		}

		[Test]
		public void IsCreated()
		{
			var repo = new MockRepository("");
			var newMock = new MockDataReturn() { Name = "UnitTestzzzz" };
			var dataResult = repo.Create(newMock);
			Assert.IsTrue(dataResult.IsSuccessful);
			var fullRetrieveTest = repo.Retrieve(dataResult.ToSuccessfulObject().Id).ToSuccessfulObject();
			Assert.IsTrue(fullRetrieveTest.Name == newMock.Name);
			Assert.IsTrue(fullRetrieveTest.Id > 0);
		}

		[Test]
		public void IsRetrieved()
		{
			var result = new MockRepository("").Retrieve(1);
			Assert.IsTrue(result.IsSuccessful);
			Assert.IsTrue(result.ToSuccessfulObject().Id == 1);
		}

		[Test]
		public void IsUpdated()
		{
			var repo = new MockRepository("");
			var result = repo.Retrieve(repo.TableOfData[0].Id).ToSuccessfulObject();
			var oldName = new string(result.Name.ToCharArray());
			result.Name = "NEW";
			var updatedResult = repo.Update(result);
			result.Name = oldName;
			Assert.IsTrue(updatedResult.ToSuccessfulObject().Name == "NEW");
			Assert.IsTrue(result.Name != updatedResult.ToSuccessfulObject().Name);
		}


		[Test]
		public void IsDeleted()
		{
			var repo = new MockRepository("");
			var toBeDeleted = repo.Retrieve(repo.TableOfData[0].Id).ToSuccessfulObject();
			repo.Delete(toBeDeleted.Id);
			var failedRetrieve = repo.Retrieve(toBeDeleted.Id);
			Assert.IsFalse(failedRetrieve.IsSuccessful);
		}

		[Test]
		public void AutoParameterTransformDefaultsWork()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapParameters(mySampleProc, new MockDataReturn());
			Assert.IsTrue(mySampleProc.Parameters.Count == 5); //the number of singular properties on MockDataReturn
			Assert.IsTrue(mySampleProc.Parameters.Contains("@ID"));
			Assert.IsFalse(mySampleProc.Parameters.Contains("@SampleList"));
		}

		[Test]
		public void AutoParameterTransformCleaningWorks()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapParameters(mySampleProc, new MockDataReturn(), true);
			Assert.IsTrue(mySampleProc.Parameters.Count == 1); //Id is an int so it defaults to zero, which should be considered valid by default
		}

		[Test]
		public void AutoParameterTransformAndIntCleaningWorks()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapParameters(mySampleProc, new MockDataReturn(), false, true);
			Assert.IsTrue(mySampleProc.Parameters.Count == 4); //Id is only int prop and was zero

			var testProc2 = new SqlCommand();
			repo.AutoMapParameters(mySampleProc, new MockDataReturn(), true, true);
			Assert.IsTrue(testProc2.Parameters.Count == 0); //All cleaned up
		}

		[Test]
		public void AutoParameterTransformIdAsOutputWorks()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapParameters(mySampleProc, new MockDataReturn(), true, true, true);
			Assert.IsTrue(mySampleProc.Parameters.Count == 1); //the Id param was detected as output and not cleaned
			var outId = mySampleProc.Parameters["@ID"];
			Assert.IsTrue(outId.Direction == ParameterDirection.Output);
		}

		[Test]
		public void SearchParameterMapDefaultsWork()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapSearch(mySampleProc, new MockDataCriteria() { IdToOptionallySearch = Guid.Empty }); //Guid.Empty only stripped on non-nullable guids.  Nullable params mean something had to hard set default values and for whatever reason actually wants these defaults
			Assert.IsTrue(mySampleProc.Parameters.Count == 2);
			//SillyParameterDefault system defaults to Guid.Empty and so it's auto-stripped. 
			//IdToOptionallySearch is hard set to Guid.Empty so it stays.  
			//SillyIntDefault system defaults to zero, but is not cleared automatically because of all the possible cases one could legitimately want to use 0 as a param.
			//The individual repo would need to deal with this if it's an unavoidable condition
			//Paging Parameters are specifically handled because of the IStandardCriteria interface
		}

		[Test]
		public void StandardFilterCriteriaMappingWorks()
		{
			var repo = new MockRepository("");
			var mySampleProc = new SqlCommand();
			repo.AutoMapSearch(mySampleProc, new MockDataCriteria() { SillyIntDefault = 5 }, true, true);
			Assert.IsTrue(mySampleProc.Parameters.Contains("@FilterForSillyIntDefault"));
		}

	}
}
