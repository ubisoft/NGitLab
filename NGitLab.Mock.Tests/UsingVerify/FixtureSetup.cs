using NUnit.Framework;

namespace NGitLab.Mock.Tests.UsingVerify;

[SetUpFixture]
public class FixtureSetup
{
    [OneTimeSetUp]
    public void Setup()
    {
        VerifyTests.VerifierSettings.AddExtraSettings(serializer =>
        {
            var converters = serializer.Converters;
            converters.Add(new Sha1Converter());
        });
    }
}
