using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Mock.Tests;

public class RepositoryMockTests
{
    [Test]
    public void Test_create_commit_in_new_branch_fails_if_both_start_branch_and_sha_specified()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        var startBranch = project.DefaultBranch;

        Assert.That(() => project.Repository.Commit(new()
        {
            Branch = "new-branch",
            StartBranch = startBranch,
            StartSha = initCommit.Sha,
            CommitMessage = "First commit in new branch",
            Actions =
            [
                new()
                {
                    Action = "update",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        }), Throws.TypeOf<GitLabException>()
                  .With.Message.Contains("GitLab server returned an error (BadRequest): start_branch, start_sha are mutually exclusive."));
    }

    [Test]
    public void Test_create_a_new_commit_with_start_branch_fails_if_branch_already_exists()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        project.Repository.CreateBranch("new-branch", initCommit.Sha);

        var startBranch = project.DefaultBranch;
        var newBranch = "new-branch";

        // Act & Assert
        Assert.That(() => project.Repository.Commit(new()
        {
            Branch = newBranch,
            StartBranch = startBranch,
            CommitMessage = "First commit in new branch",
            Actions =
            [
                new()
                {
                    Action = "update",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        }), Throws.TypeOf<GitLabException>()
                  .With.Message.Contains($"A branch called '{newBranch}' already exists."));
    }

    [Test]
    public void Test_create_a_new_commit_with_start_sha_fails_if_branch_already_exists()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        project.Repository.CreateBranch("new-branch", initCommit.Sha);

        var startBranch = project.DefaultBranch;
        var newBranch = "new-branch";

        // Act & Assert
        Assert.That(() => project.Repository.Commit(new()
        {
            Branch = newBranch,
            StartSha = initCommit.Sha,
            CommitMessage = "First commit in new branch",
            Actions =
            [
                new()
                {
                    Action = "create",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        }), Throws.TypeOf<GitLabException>()
                  .With.Message.Contains($"A branch called '{newBranch}' already exists."));
    }

    [Test]
    public void Test_create_a_new_commit_on_new_branch_with_start_branch()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        var startBranch = project.DefaultBranch;
        var newBranch = "new-branch";

        // Act & Assert
        var commitMessage = "First commit in new branch";
        var newCommit = project.Repository.Commit(new()
        {
            Branch = newBranch,
            StartBranch = startBranch,
            CommitMessage = commitMessage,
            Actions =
            [
                new()
                {
                    Action = "create",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        });
        Assert.That(newCommit.Message.Trim(), Is.EqualTo(commitMessage));
    }

    [Test]
    public void Test_create_a_new_commit_on_new_branch_with_start_sha()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        var newBranch = "new-branch";

        // Act & Assert
        var commitMessage = "First commit in new branch";
        var newCommit = project.Repository.Commit(new()
        {
            Branch = newBranch,
            StartSha = initCommit.Sha,
            CommitMessage = commitMessage,
            Actions =
            [
                new()
                {
                    Action = "create",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        });
        Assert.That(newCommit.Message.Trim(), Is.EqualTo(commitMessage));
    }

    [Test]
    public void Test_create_a_new_commit_on_nonexistent_branch()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        // Act & Assert
        var commitMessage = "First commit in non existing branch";
        Assert.That(() => project.Repository.Commit(new()
        {
            Branch = "new-branch",
            CommitMessage = commitMessage,
            Actions =
            [
                new()
                {
                    Action = "create",
                    Content = "This is in a new branch",
                    FilePath = "README.md",
                },
            ],
        }), Throws.TypeOf<GitLabException>()
                  .With.Message.Contains("You can only create or edit files when you are on a branch."));
    }

    [Test]
    public void Test_create_a_new_commit_on_existing_branch()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        // Act & Assert
        var commitMessage = "First commit in existing branch";
        var newCommit = project.Repository.Commit(new()
        {
            Branch = project.DefaultBranch,
            CommitMessage = commitMessage,
            Actions =
            [
                new()
                {
                    Action = "create",
                    Content = "This is in an existing branch",
                    FilePath = "README.md",
                },
            ],
        });
        Assert.That(newCommit.Message.Trim(), Is.EqualTo(commitMessage));
    }

    [Test]
    public async Task Test_commit_diff_returns_diff_information()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        var mainCommit = project.Repository.Commit(user, "Main commit");

        project.Repository.CreateAndCheckoutBranch("to-be-merged");
        project.Repository.Commit(user, "branch commit");
        var mr = project.CreateMergeRequest(user, "Merge request commit", "mr description", project.DefaultBranch, "to-be-merged");
        mr.Accept(user);

        // Act
        var client = server.CreateClient(user);
        var diff = await client.GetRepository(project.Id).CompareAsync(new(initCommit.Sha, mr.MergeCommitSha!.ToString()));

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(diff.Diff, Is.Not.Empty);
            Assert.That(diff.Commits.Select(c => c.Id), Is.EqualTo([new Sha1(mainCommit.Sha), mr.MergeCommitSha]));
            Assert.That(diff.Commit?.Id, Is.EqualTo(mr.MergeCommitSha));
        }
    }

    [Test]
    public async Task Test_commit_diff_same_commit_returns_empty_diff()
    {
        // Arrange
        using var server = new GitLabServer();
        var user = server.Users.AddNew();
        var project = user.Namespace.Projects.AddNew(project => project.Visibility = VisibilityLevel.Internal);
        var initCommit = project.Repository.Commit(user, "Initial commit");

        // Act & assert
        var client = server.CreateClient(user);
        var diff = await client.GetRepository(project.Id).CompareAsync(new(initCommit.Sha, initCommit.Sha));

        Assert.That(diff.Diff, Is.Empty);
        Assert.That(diff.Commits, Is.Empty);
        Assert.That(diff.Commit, Is.Null);
    }
}
