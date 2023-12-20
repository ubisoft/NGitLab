using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class LintCITests
{
    [TestCase(null, "Pipeline filtered out by workflow rules.")]
    [TestCase("main", "Pipeline filtered out by workflow rules.")]
    [TestCase("dummy", "Reference not found")]
    public async Task Test_GetLintCIByRef(string @ref, string expectedError)
    {
        // Arrange
        using var server = new GitLabServer();

        var user = server.Users.AddNew("user1");
        var client = server.CreateClient(user);

        var group = new Group("SomeGroup");
        server.Groups.Add(group);

        var project = new Project("Project") { Visibility = VisibilityLevel.Internal };
        group.Projects.Add(project);

        // Simulate what GitLab would return if CI was configured not to run pipelines in "main" branch
        project.LintCIs.Add(new LintCI(project.DefaultBranch, valid: false, "Pipeline filtered out by workflow rules."));

        // Act
        var result = await client.Lint.ValidateProjectCIConfigurationAsync(project.Id.ToString(CultureInfo.InvariantCulture), new LintCIOptions
        {
            DryRun = true,
            Ref = @ref,
        });

        // Assert
        Assert.That(result.Valid, Is.False);
        Assert.That(result.Errors.Single(), Is.EqualTo(expectedError));
    }
}
