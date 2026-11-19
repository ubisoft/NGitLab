using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using NUnit.Framework;
using PublicApiGenerator;
using VerifyNUnit;
using VerifyTests;

namespace NGitLab.Tests.Internals;

public class AssemblyTests
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

        // Act
        var publicApi = assembly.GeneratePublicApi(apiGenOptions);

        // Assert
        _ = await Verifier.Verify(publicApi, verifySettings);
    }
}
