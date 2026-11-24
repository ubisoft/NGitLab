using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using NUnit.Framework;
using PublicApiGenerator;
using VerifyNUnit;
using VerifyTests;

namespace NGitLab.PublicApi.Tests;

public class NGitLabApiTests
{
    [Test]
    public async Task MakeSurePublicApiHasNotChanged()
    {
        // Arrange
        var assembly = typeof(GitLabClient).Assembly;

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
        verifySettings.UniqueForTargetFrameworkAndVersion(); // The public API differs from one target framework to another (e.g. net6.0 uses the netstandard2.0 build)

        // RepositoryUrl is derived from the git remote / CI environment (with or without ".git"),
        // so it differs between local machines and CI. It is not part of the public API.
        verifySettings.ScrubLinesContaining("AssemblyMetadata(\"RepositoryUrl\"");

        // Act
        var publicApi = assembly.GeneratePublicApi(apiGenOptions);

        // Assert
        _ = await Verifier.Verify(publicApi, verifySettings);
    }
}
