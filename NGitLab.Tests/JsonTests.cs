using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;
using NUnit.Framework;

namespace NGitLab.Tests;

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
        var parsedValue = Serializer.Deserialize<TestEnum>('"' + value + '"');
        Assert.That(parsedValue, Is.EqualTo(expectedValue));
    }

    [TestCase("dfsf")]
    public void DeserializeEnumWithEnumMemberAttribute_UnknownValues(string value)
    {
        Assert.That(() => Serializer.Deserialize<TestEnum>('"' + value + '"'), Throws.Exception);
    }

    [Test]
    public void DeserializeNewerContract_Ok()
    {
        var newContractObject = new TestContractV2 { Id = 100, Title = "Newer Contract" };
        var newContractJson = Serializer.Serialize(newContractObject);

        TestContractV1 oldContractObject = null;
        Assert.DoesNotThrow(() => oldContractObject = Serializer.Deserialize<TestContractV1>(newContractJson));
        Assert.That(oldContractObject, Is.Not.Null);
    }

    [Test]
    public void DeserializeOlderContract_Ok()
    {
        var oldContractObject = new TestContractV1 { Title = "Older Contract" };
        var oldContractJson = Serializer.Serialize(oldContractObject);

        TestContractV2 newContractObject = null;
        Assert.DoesNotThrow(() => newContractObject = Serializer.Deserialize<TestContractV2>(oldContractJson));
        Assert.That(newContractObject, Is.Not.Null);
    }

    public class TestContractV1
    {
        [JsonPropertyName("title")]
        public string Title;
    }

    public class TestContractV2
    {
        [JsonPropertyName("title")]
        public string Title;

        [JsonPropertyName("id")]
        public long Id;
    }
}
