using System.Runtime.CompilerServices;
using VerifyTests;

namespace NGitLab.Mock.Tests.UsingVerify;

internal static class VerifyInitializer
{
    // Must run before any Verify call, so it can't be an NUnit [OneTimeSetUp]
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifierSettings.AddExtraSettings(serializer => serializer.Converters.Add(new Sha1Converter()));
    }
}
