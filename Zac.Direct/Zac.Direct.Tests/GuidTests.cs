using System;
using NUnit.Framework;

namespace Zac.Direct.Tests
{
    public class GuidTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmptyGuidIsInvalid()
        {
            Assert.IsFalse(Helper.IsValidGuid(Guid.Empty));
        }

        [Test]
        public void EmptyStringIsInvalid()
        {
            Assert.IsFalse(Helper.IsValidGuid(""));
        }

        [Test]
        public void NullIsInvalid()
        {
            Assert.IsFalse(Helper.IsValidGuid((Guid?)null));
        }

        [Test]
        public void AnyGuidIsValid()
        {
            Assert.IsTrue(Helper.IsValidGuid(Guid.NewGuid()));
        }

        [Test]
        public void AnyGuidStringIsValid()
        {
            var anyGuid = Guid.NewGuid();
            Assert.IsTrue(Helper.IsValidGuid(anyGuid.ToString()));
            Assert.IsTrue(Helper.IsValidGuid(anyGuid.ToString("N")));
        }

        [Test]
        public void ConvertedGuidIsCorrect()
        {
            var anyGuid = Guid.NewGuid().ToString("N");
            Guid convertedGuid;
            if (Helper.IsValidGuid(anyGuid, out convertedGuid))
            {
                Assert.AreEqual(Guid.Parse(anyGuid), convertedGuid);
            }
            else
            {
                Assert.Fail("Converted Guid hit unexpected case");
            }
        }
    }
}