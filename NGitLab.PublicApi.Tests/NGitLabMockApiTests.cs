using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using NGitLab.Mock;
using NUnit.Framework;
using PublicApiGenerator;
using VerifyNUnit;
using VerifyTests;

namespace NGitLab.PublicApi.Tests;

public class NGitLabMockApiTests
{
    [Test]
    public async Task MakeSurePublicApiHasNotChanged()
    {
        // Arrange
        var assembly = typeof(GitLabServer).Assembly;

        var apiGenOptions = new ApiGeneratorOptions
        {
            ExcludeAttributes =
            [
                typeof(InternalsVisibleToAttribute).FullName,
                "System.Runtime.CompilerServices.IsByRefLike",
                typeof(TargetFrameworkAttribute).FullName,
            ],
        };

        var verifySettings = new VerifySettings();
        verifySettings.UseDirectory("snapshots");

        // RepositoryUrl is derived from the git remote / CI environment (with or without ".git"),
        // so it differs between local machines and CI. It is not part of the public API.
        verifySettings.ScrubLinesContaining("AssemblyMetadata(\"RepositoryUrl\"");

        // Act
        var publicApi = assembly.GeneratePublicApi(apiGenOptions);

        // Assert
        _ = await Verifier.Verify(publicApi, verifySettings);
    }
}
