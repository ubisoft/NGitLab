using System.Diagnostics.CodeAnalysis;
using NGitLab.Impl.Json;
using NUnit.Framework;

namespace NGitLab.Tests.Impl
{
    public class SimpleJsonTests
    {
        [Test]
        public void Test_basic_field()
        {
            var model = Load("{ \"BasicField\":\"asd\" }");

            Assert.That(model.BasicField, Is.EqualTo("asd"));
        }

        [Test]
        public void Test_dynamic_enums_fill_the_string_value_when_the_enum_is_unknown()
        {
            var model = Load("{ \"MyEnum\":\"unknown\" }");

            Assert.That(model.MyEnum.StringValue, Is.EqualTo("unknown"));
            Assert.That(model.MyEnum.EnumValue, Is.Null);
        }

        [Test]
        public void Test_dynamic_enums_fill_the_enum_value_when_the_enum_is_unknown()
        {
            var model = Load("{ \"MyEnum\":\"KnownField\" }");

            Assert.That(model.MyEnum.StringValue, Is.Null);
            Assert.That(model.MyEnum, Is.EqualTo(MockEnum.KnownField));
        }

        private static MockModel Load(string json)
        {
            return Serializer.Deserialize<MockModel>(json);
        }

        [SuppressMessage("Design", "CA1812: Avoid uninstantiated internal classes", Justification = "The class is instantiated dynamically using Serializer")]
        private sealed class MockModel
        {
            public string BasicField { get; set; }

            public DynamicEnum<MockEnum> MyEnum { get; set; }
        }

        public enum MockEnum
        {
            Default,
            KnownField,
        }
    }
}
