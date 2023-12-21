using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class CommitsTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_can_get_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);

        var commit = context.Client.GetCommits(project.Id).GetCommit(project.DefaultBranch);
        Assert.That(commit.Message, Is.Not.Null);
        Assert.That(commit.ShortId, Is.Not.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_get_stats_in_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        context.Client.GetRepository(project.Id).Files.Create(new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "file to be updated",
            Path = "CommitStats.txt",
            RawContent = "I'm defective and i need to be fixeddddddddddddddd",
        });

        context.Client.GetRepository(project.Id).Files.Update(new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "fixing the file",
            Path = "CommitStats.txt",
            RawContent = "I'm no longer defective and i have been fixed\n\n\r\n\r\rEnjoy",
        });

        var commit = context.Client.GetCommits(project.Id).GetCommit(project.DefaultBranch);
        Assert.That(commit.Stats.Additions, Is.EqualTo(4));
        Assert.That(commit.Stats.Deletions, Is.EqualTo(1));
        Assert.That(commit.Stats.Total, Is.EqualTo(5));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_get_merge_request_associated_to_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();

        context.Client.GetRepository(project.Id).Branches.Create(new BranchCreate { Name = "test-mr", Ref = project.DefaultBranch });

        var commit = context.Client.GetCommits(project.Id).Create(new CommitCreate
        {
            Branch = "test-mr",
            CommitMessage = "Test to retrieve MR from commit sha",
        });

        var mergeRequestTitle = "Test to retrieve MR from commit sha";
        context.Client.GetMergeRequest(project.Id).Create(new MergeRequestCreate
        {
            SourceBranch = "test-mr",
            TargetBranch = project.DefaultBranch,
            Title = mergeRequestTitle,
        });

        var mergeRequests = await GitLabTestContext.RetryUntilAsync(
            () => context.Client.GetCommits(project.Id).GetRelatedMergeRequestsAsync(new RelatedMergeRequestsQuery { Sha = commit.Id }),
            mergeRequests => mergeRequests.Any(),
            TimeSpan.FromSeconds(10));

        var mergeRequest = mergeRequests.Single();
        Assert.That(mergeRequest.Title, Is.EqualTo(mergeRequestTitle));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_can_cherry_pick_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var repository = context.Client.GetRepository(project.Id);
        var commitClient = context.Client.GetCommits(project.Id);

        repository.Branches.Create(new BranchCreate { Name = "test-cherry-pick", Ref = project.DefaultBranch });

        var commit = commitClient.Create(new CommitCreate
        {
            Branch = "test-cherry-pick",
            CommitMessage = "Test to cherry-pick",
            Actions = new List<CreateCommitAction>
            {
                new()
                {
                    Action = "update",
                    Content = "Test to cherry-pick",
                    FilePath = "README.md",
                },
            },
        });

        var cherryPickedCommit = commitClient.CherryPick(new CommitCherryPick
        {
            Branch = project.DefaultBranch,
            Sha = commit.Id,
        });

        var latestCommit = commitClient.GetCommit(project.DefaultBranch);
        Assert.That(latestCommit.Id, Is.EqualTo(cherryPickedCommit.Id));
    }
}
