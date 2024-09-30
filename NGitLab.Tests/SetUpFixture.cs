using NGitLab.Extensions;
using NUnit.Framework;

namespace NGitLab.Tests;

[SetUpFixture]
public sealed class SetUpFixture
{
    [OneTimeSetUp]
    public void RunBeforeAnyTests()
    {
        FunctionRetryExtensions.Logger = msg => TestContext.Out.WriteLine($"[{TestContext.CurrentContext.Test.FullName}] {msg}");
    }
}
