using NUnit.Framework;

namespace NGitLab.Tests.Impl;

public class DynamicEnumTests
{
    [Test]
    public void Test_comparison()
    {
        Assert.That(new DynamicEnum<MockEnum>(MockEnum.A), Is.EqualTo(MockEnum.A));
        Assert.That(new DynamicEnum<MockEnum>(MockEnum.A), Is.Not.EqualTo(MockEnum.B));
        Assert.That(new DynamicEnum<MockEnum>("unknown").StringValue, Is.EqualTo("unknown"));
        Assert.That(new DynamicEnum<MockEnum>("unknown").StringValue, Is.Not.EqualTo("other"));
    }

    public enum MockEnum
    {
        A,
        B,
    }
}
