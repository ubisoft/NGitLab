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

        [Test]
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

        [Test]
        [TestCase("dfsf")]
        public void DeserializeEnumWithEnumMemberAttribute_UnknownValues(string value)
        {
            Assert.Throws<Exception>(() => SimpleJson.DeserializeObject<TestEnum>('"' + value + '"'));
        }
    }
}
