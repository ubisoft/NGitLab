using System.Threading.Tasks;
using NUnit.Framework;

namespace NGitLab.Tests;

public sealed class GitLabClientTests
{
    [Test]
    public async Task ShouldWorkWithoutToken()
    {
        // https://gitlab.com/gitlab-org/gitlab
        const long GitLabProjectId = 278964;

        var client = new GitLabClient("https://gitlab.com");
        var project = await client.Projects.GetByIdAsync(GitLabProjectId, new Models.SingleProjectQuery { Statistics = false });

        Assert.That(project, Is.Not.Null);
        Assert.That(project.Id, Is.EqualTo(GitLabProjectId));
        Assert.That(project.Name, Is.EqualTo("GitLab"));
    }
}
