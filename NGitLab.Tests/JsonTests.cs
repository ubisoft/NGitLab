using System;
using System.Runtime.Serialization;
using NGitLab.Impl;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class JsonTests
    {
        public enum TestEnum
        {
            [EnumMember(Value = "v1")]
            Value1,

            [EnumMember(Value = "v2")]
            Value2,
        }

        [TestCase("v1", TestEnum.Value1)]
        [TestCase("v2", TestEnum.Value2)]
        [TestCase("V1", TestEnum.Value1)]
        [TestCase("V2", TestEnum.Value2)]
        [TestCase("value2", TestEnum.Value2)]
        public void DeserializeEnumWithEnumMemberAttribute_Ok(string value, TestEnum expectedValue)
        {
            var parsedValue = SimpleJson.DeserializeObject<TestEnum>('"' + value + '"');
            Assert.AreEqual(expectedValue, parsedValue);
        }

        [TestCase("dfsf")]
        public void DeserializeEnumWithEnumMemberAttribute_UnknownValues(string value)
        {
            Assert.Throws<Exception>(() => SimpleJson.DeserializeObject<TestEnum>('"' + value + '"'));
        }

        [Test]
        public void DeserializeNewerContract_Ok()
        {
            var newContractObject = new TestContractV2 { Id = 100, Title = "Newer Contract" };
            var newContractJson = SimpleJson.SerializeObject(newContractObject);

            TestContractV1 oldContractObject = null;
            Assert.DoesNotThrow(() => oldContractObject = SimpleJson.DeserializeObject<TestContractV1>(newContractJson));
            Assert.NotNull(oldContractObject);
        }

        [Test]
        public void DeserializeOlderContract_Ok()
        {
            var oldContractObject = new TestContractV1 { Title = "Older Contract" };
            var oldContractJson = SimpleJson.SerializeObject(oldContractObject);

            TestContractV2 newContractObject = null;
            Assert.DoesNotThrow(() => newContractObject = SimpleJson.DeserializeObject<TestContractV2>(oldContractJson));
            Assert.NotNull(newContractObject);
        }

        [DataContract]
        public class TestContractV1
        {
            [DataMember(Name = "title")]
            public string Title;
        }

        [DataContract]
        public class TestContractV2
        {
            [DataMember(Name = "title")]
            public string Title;

            [DataMember(Name = "id")]
            public int Id;
        }
    }
}
