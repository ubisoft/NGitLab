using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NGitLab.Impl.Json;
using NUnit.Framework;

namespace NGitLab.Tests.Impl
{
    public class JsonConverterTests
    {
        [Test]
        public void Test_DeserializeNullToSupportedValueType_Succeeds()
        {
            var json = @"{
  ""a_boolean"": null,
  ""a_date_time"": null,
  ""a_double"": null,
  ""an_int32"": null,
  ""an_int64"": null
}";
            var obj = Serializer.Deserialize<MyDataContract>(json);

            Assert.NotNull(obj);
            Assert.AreEqual(false, obj.SomeBool);
            Assert.AreEqual(DateTime.MinValue, obj.SomeDateTime);
            Assert.AreEqual(0.0, obj.SomeDouble);
            Assert.AreEqual(0, obj.SomeInt32);
            Assert.AreEqual(0L, obj.SomeInt64);
        }

        [Test]
        public void Test_DeserializeNullToUnsupportedValueType_Throws()
        {
            var json = @"{ ""a_uint32"": null }";
            var ex = Assert.Throws<JsonException>(() => Serializer.Deserialize<MyDataContract>(json));
            StringAssert.StartsWith("The JSON value could not be converted to System.UInt32.", ex.Message);
        }

        [Test]
        public void Test_DeserializeStringToInt32()
        {
            var json = @"{ ""an_int32"": ""1234"" }";
            var obj = Serializer.Deserialize<MyDataContract>(json);
            Assert.NotNull(obj);
            Assert.AreEqual(1234, obj.SomeInt32);
        }

        [Test]
        public void Test_DeserializeStringToInt64_Succeeds()
        {
            var json = @"{ ""an_int64"": ""-1234"" }";
            var obj = Serializer.Deserialize<MyDataContract>(json);
            Assert.NotNull(obj);
            Assert.AreEqual(-1234L, obj.SomeInt64);
        }

        [Test]
        public void Test_DeserializeStringToDouble_Succeeds()
        {
            var json = @"{ ""a_double"": ""-1234.5"" }";
            var obj = Serializer.Deserialize<MyDataContract>(json);
            Assert.NotNull(obj);
            Assert.AreEqual(-1234.5d, obj.SomeDouble);
        }

        [TestCase("2022-01-12", DateTimeKind.Unspecified)]
        [TestCase("2022-01-12T22:49:21.552Z", DateTimeKind.Utc)]
        [TestCase("2022-01-12T22:49:21.552+00:00", DateTimeKind.Utc)]
        public void Test_DeserializeStringToDateTime_SupportsMultipleFormats(string input, DateTimeKind kind)
        {
            var json = $@"{{ ""a_date_time"": ""{input}"" }}";
            var obj = Serializer.Deserialize<MyDataContract>(json);
            Assert.NotNull(obj);
            Assert.AreEqual(kind, obj.SomeDateTime.Kind);
            Assert.AreEqual(new DateTime(2022, 1, 12).Date, obj.SomeDateTime.Date);
        }

        public class MyDataContract
        {
            [JsonPropertyName("a_boolean")]
            public bool SomeBool { get; set; }

            [JsonPropertyName("a_date_time")]
            public DateTime SomeDateTime { get; set; }

            [JsonPropertyName("a_double")]
            public double SomeDouble { get; set; }

            [JsonPropertyName("an_int32")]
            public int SomeInt32 { get; set; }

            [JsonPropertyName("an_int64")]
            public long SomeInt64 { get; set; }

            [JsonPropertyName("a_uint32")]
            public uint SomeUInt32 { get; set; }
        }
    }
}
