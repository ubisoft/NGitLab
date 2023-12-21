using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class BranchClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_CommitInfoIsCorrectlyDeserialized()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branchClient = context.Client.GetRepository(project.Id).Branches;
        var currentUser = context.Client.Users.Current;

        var defaultBranch = branchClient[project.DefaultBranch];
        Assert.That(defaultBranch, Is.Not.Null);

        var commit = defaultBranch.Commit;
        Assert.That(commit, Is.Not.Null);

        Assert.That(commit.Id.ToString(), Has.Length.EqualTo(40));
        Assert.That(commit.ShortId, Has.Length.GreaterThan(7));

        var fiveMinutesAgo = DateTime.UtcNow - TimeSpan.FromMinutes(5);
        Assert.That(commit.CreatedAt, Is.GreaterThan(fiveMinutesAgo));

        Assert.That(commit.Parents, Has.Length.EqualTo(1));

        Assert.That(commit.Title, Is.EqualTo("add test file 2"));
        Assert.That(commit.Message, Is.EqualTo("add test file 2"));

        Assert.That(commit.AuthorName, Is.EqualTo(currentUser.Name));
        Assert.That(commit.AuthorEmail, Is.EqualTo(currentUser.Email));
        Assert.That(fiveMinutesAgo, Is.LessThan(commit.AuthoredDate));

        Assert.That(commit.CommitterName, Is.EqualTo(currentUser.Name));
        Assert.That(commit.CommitterEmail, Is.EqualTo(currentUser.Email));
        Assert.That(fiveMinutesAgo, Is.LessThan(commit.CommittedDate));

        Assert.That(Uri.TryCreate(commit.WebUrl, UriKind.Absolute, out _), Is.True);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_search_branches()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var branchClient = context.Client.GetRepository(project.Id).Branches;

        var defaultBranch = project.DefaultBranch;

        var branches = branchClient.Search(defaultBranch);
        var expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo(defaultBranch));

        // This case only worked with GitLab 15.7 and later
        // https://gitlab.com/gitlab-org/gitlab/-/merge_requests/104451
        /*
        branches = branchClient.Search($"^{defaultBranch}$");
        expectedBranch = branches.Single();
        Assert.AreEqual(defaultBranch, expectedBranch.Name);
        */

        branches = branchClient.Search($"^{defaultBranch[..^1]}");
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo(defaultBranch));

        branches = branchClient.Search($"{defaultBranch[1..]}$");
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo(defaultBranch));

        branches = branchClient.Search(defaultBranch[1..^1]);
        expectedBranch = branches.Single();
        Assert.That(expectedBranch.Name, Is.EqualTo(defaultBranch));

        branches = branchClient.Search("foobar");
        Assert.That(branches, Is.Empty);
    }
}
