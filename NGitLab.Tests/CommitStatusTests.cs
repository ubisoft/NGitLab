using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class CommitStatusTests
{
    private sealed class CommitStatusTestContext : IDisposable
    {
        public GitLabTestContext Context { get; }

        public Project Project { get; }

        public Commit Commit { get; }

        public ICommitStatusClient CommitStatusClient { get; }

        public CommitStatusTestContext(GitLabTestContext context, Project project, Commit commit, ICommitStatusClient commitStatusClient)
        {
            Context = context;
            Project = project;
            Commit = commit;
            CommitStatusClient = commitStatusClient;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public static async Task<CommitStatusTestContext> Create()
        {
            var context = await GitLabTestContext.CreateAsync().ConfigureAwait(false);
            var project = context.CreateProject();

            var upsert = new FileUpsert
            {
                RawContent = "test",
                CommitMessage = "Commit for CommitStatusTests",
                Path = "CommitStatusTests.txt",
                Branch = project.DefaultBranch,
            };
            context.Client.GetRepository(project.Id).Files.Create(upsert);
            var commit = context.Client.GetCommits(project.Id).GetCommit(upsert.Branch);
            var client = context.Client.GetCommitStatus(project.Id);
            return new CommitStatusTestContext(context, project, commit, client);
        }

        public CommitStatusCreate AddOrUpdateCommitStatus(string state = "success", int? coverage = null)
        {
            var commitStatus = SetupCommitStatus(state, coverage);

            var createdCommitStatus = CommitStatusClient.AddOrUpdate(commitStatus);

            Assert.That(createdCommitStatus.Ref, Is.EqualTo(commitStatus.Ref));
            Assert.That(createdCommitStatus.Coverage, Is.EqualTo(commitStatus.Coverage));
            Assert.That(createdCommitStatus.Description, Is.EqualTo(commitStatus.Description));
            Assert.That(createdCommitStatus.Status, Is.EqualTo(commitStatus.State));
            Assert.That(createdCommitStatus.Name, Is.EqualTo(commitStatus.Name));
            Assert.That(createdCommitStatus.TargetUrl, Is.EqualTo(commitStatus.TargetUrl));
            Assert.That(string.Equals(commitStatus.CommitSha, createdCommitStatus.CommitSha, StringComparison.OrdinalIgnoreCase), Is.True);

            return createdCommitStatus;
        }

        private CommitStatusCreate SetupCommitStatus(string state, int? coverage = 100)
        {
            return new CommitStatusCreate
            {
                Ref = Project.DefaultBranch,
                CommitSha = Commit.Id.ToString(),
                Name = "Commit for CommitStatusTests",
                State = state,
                Description = "desc",
                Coverage = coverage,
                TargetUrl = "https://google.ca/",
            };
        }
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_commit_status()
    {
        using var context = await CommitStatusTestContext.Create();
        var createdCommitStatus = context.AddOrUpdateCommitStatus();

        var commitStatus = context.CommitStatusClient.AllBySha(context.Commit.Id.ToString().ToLowerInvariant()).ToList();
        Assert.That(commitStatus.FirstOrDefault()?.Status, Is.Not.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_with_no_coverage()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(coverage: null);

        Assert.That(commitStatus.Coverage, Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_pending_to_running_to_success()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
        Assert.That(commitStatus.Status, Is.EqualTo("pending"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "running");
        Assert.That(commitStatus.Status, Is.EqualTo("running"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "success");
        Assert.That(commitStatus.Status, Is.EqualTo("success"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_pending_to_failed()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
        Assert.That(commitStatus.Status, Is.EqualTo("pending"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "failed");
        Assert.That(commitStatus.Status, Is.EqualTo("failed"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_pending_to_canceled()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
        Assert.That(commitStatus.Status, Is.EqualTo("pending"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
        Assert.That(commitStatus.Status, Is.EqualTo("canceled"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_success_to_pending()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
        Assert.That(commitStatus.Status, Is.EqualTo("success"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
        Assert.That(commitStatus.Status, Is.EqualTo("pending"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_success_to_failed()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
        Assert.That(commitStatus.Status, Is.EqualTo("success"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "failed");
        Assert.That(commitStatus.Status, Is.EqualTo("failed"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_success_to_canceled()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "success");
        Assert.That(commitStatus.Status, Is.EqualTo("success"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
        Assert.That(commitStatus.Status, Is.EqualTo("canceled"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_post_commit_status_and_update_it_from_canceled_to_pending()
    {
        using var context = await CommitStatusTestContext.Create();
        var commitStatus = context.AddOrUpdateCommitStatus(state: "canceled");
        Assert.That(commitStatus.Status, Is.EqualTo("canceled"));

        commitStatus = context.AddOrUpdateCommitStatus(state: "pending");
        Assert.That(commitStatus.Status, Is.EqualTo("pending"));
    }
}
