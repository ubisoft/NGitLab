using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;
using NUnit.Framework;

namespace NGitLab.Tests;

public class JsonTests
{
    private const string Json = """
        {
          "title": "Newer Contract",
          "id": 100
        }
        """;

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
    public void DeserializeToTestContractV1_Ok()
    {
        // Act
        var testContractV1Object = Serializer.Deserialize<TestContractV1>(Json);

        // Assert
        Assert.That(testContractV1Object, Is.Not.Null);
        Assert.That(testContractV1Object.Title, Is.EqualTo("Newer Contract"));
    }

    [Test]
    public void DeserializeToTestContractV2_Ok()
    {
        // Act
        var testContractV2Object = Serializer.Deserialize<TestContractV2>(Json);

        // Assert
        Assert.That(testContractV2Object, Is.Not.Null);
        Assert.That(testContractV2Object.Title, Is.EqualTo("Newer Contract"));
        Assert.That(testContractV2Object.Id, Is.EqualTo(100));
    }

    public class TestContractV1
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class TestContractV2
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }
    }
}
