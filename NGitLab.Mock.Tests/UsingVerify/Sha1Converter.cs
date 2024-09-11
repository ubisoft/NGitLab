using VerifyTests;

namespace NGitLab.Mock.Tests.UsingVerify;

internal class Sha1Converter : WriteOnlyJsonConverter<Sha1>
{
    public override void Write(VerifyJsonWriter writer, Sha1 value) =>
        writer.WriteValue(value.ToString());
}
