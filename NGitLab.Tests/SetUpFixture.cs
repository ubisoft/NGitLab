using System.Threading.Tasks;
using NGitLab.Extensions;
using NGitLab.Tests.Docker;
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

    [OneTimeTearDown]
    public async Task RunAfterAllTestsAsync()
    {
        await GitLabDockerContainer.DisposeInstanceAsync().ConfigureAwait(false);
    }
}
