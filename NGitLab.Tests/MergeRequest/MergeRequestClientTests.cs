using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NuGet.Versioning;
using NUnit.Framework;
using Polly;

namespace NGitLab.Tests;

public class MergeRequestClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_api()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        Assert.That(mergeRequestClient[mergeRequest.Iid].Id, Is.EqualTo(mergeRequest.Id), "Test we can get a merge request by IId");
        Assert.That((await mergeRequestClient.GetByIidAsync(mergeRequest.Iid, options: null)).Id, Is.EqualTo(mergeRequest.Id), "Test we can get a merge request by IId");

        ListMergeRequest(mergeRequestClient, mergeRequest);
        mergeRequest = UpdateMergeRequest(mergeRequestClient, mergeRequest);
        Test_can_update_labels_with_delta(mergeRequestClient, mergeRequest);
        Test_can_update_a_subset_of_merge_request_fields(mergeRequestClient, mergeRequest);

        await GitLabTestContext.RetryUntilAsync(
                () => mergeRequestClient[mergeRequest.Iid],
                mr => string.Equals(mr.MergeStatus, "can_be_merged", StringComparison.Ordinal),
                TimeSpan.FromSeconds(120))
            .ConfigureAwait(false);

        Assert.That(context.Client.GetRepository(project.Id).Branches[mergeRequest.SourceBranch].Protected, Is.False, "The source branch is protected but should not be");

        TestContext.Out.WriteLine("MR is ready to be merged");
        AcceptMergeRequest(mergeRequestClient, mergeRequest);
        TestContext.Out.WriteLine("MR is merged");

        // Since GitLab 13.10, this part is flaky
        // await Task.Delay(TimeSpan.FromSeconds(5)).ConfigureAwait(false);
        // await GitLabTestContext.RetryUntilAsync(
        //       () => context.Client.GetRepository(project.Id).Branches.All.ToList(),
        //       branches => branches.All(branch => !string.Equals(branch.Name, mergeRequest.SourceBranch, StringComparison.Ordinal)),
        //       TimeSpan.FromSeconds(240)) // GitLab seems very slow to delete a branch on my machine...
        //   .ConfigureAwait(false);
        var commits = mergeRequestClient.Commits(mergeRequest.Iid).All;
        Assert.That(commits.Any(), Is.True, "Can return the commits");

        if (context.IsGitLabVersionInRange(VersionRange.Parse("[15.6,)"), out _))
            Assert.That(mergeRequest.DetailedMergeStatus.EnumValue, Is.Not.Null);
        else
            Assert.That(mergeRequest.DetailedMergeStatus.EnumValue, Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_rebase()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        // Additional commit in default branch, to create divergence
        var commitClient = context.Client.GetCommits(project.Id);
        commitClient.Create(new CommitCreate
        {
            Branch = project.DefaultBranch,
            CommitMessage = "A message",
            AuthorEmail = "a@example.com",
            AuthorName = "a",
            Actions =
            {
                new CreateCommitAction
                {
                    Action = "create",
                    Content = "This is a test",
                    FilePath = "whatever.txt",
                },
            },
        });

        var mr = mergeRequestClient[mergeRequest.Iid];
        Assert.That(mr.DivergedCommitsCount, Is.EqualTo(1),
            "There should be a 1-commit divergence between the default branch NOW and its state at the moment the MR was created");

        RebaseMergeRequest(mergeRequestClient, mergeRequest);

        var commits = await GitLabTestContext.RetryUntilAsync(
            () => mergeRequestClient.Commits(mergeRequest.Iid).All,
            commits => commits.Any(),
            TimeSpan.FromSeconds(10));

        Assert.That(commits.Any(), Is.True, "Can return the commits");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_rebaseasync_skip_ci()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        // Additional commit in default branch, to create divergence
        var commitClient = context.Client.GetCommits(project.Id);
        commitClient.Create(new CommitCreate
        {
            Branch = project.DefaultBranch,
            CommitMessage = "A message",
            AuthorEmail = "a@example.com",
            AuthorName = "a",
            Actions =
            {
                new CreateCommitAction
                {
                    Action = "create",
                    Content = "This is a test",
                    FilePath = "whatever.txt",
                },
            },
        });

        var mr = mergeRequestClient[mergeRequest.Iid];
        Assert.That(mr.DivergedCommitsCount, Is.EqualTo(1),
            "There should be a 1-commit divergence between the default branch NOW and its state at the moment the MR was created");

        var rebaseResult = await mergeRequestClient.RebaseAsync(mergeRequest.Iid, new MergeRequestRebase { SkipCi = true });
        Assert.That(rebaseResult.RebaseInProgress, Is.True);

        var commits = await GitLabTestContext.RetryUntilAsync(
            () => mergeRequestClient.Commits(mergeRequest.Iid).All,
            commits => commits.Any(),
            TimeSpan.FromSeconds(10));

        Assert.That(commits.Any(), Is.True, "Can return the commits");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_id_is_not_equal_to_iid()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (_, mergeRequest) = context.CreateMergeRequest();
        Assert.That(mergeRequest.Iid, Is.Not.EqualTo(mergeRequest.Id));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_gitlab_returns_an_error_when_trying_to_create_a_request_with_same_source_and_destination()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject(initializeWithCommits: true);
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        var exception = Assert.Throws<GitLabException>(() =>
        {
            mergeRequestClient.Create(new MergeRequestCreate
            {
                Title = "ErrorRequest",
                SourceBranch = project.DefaultBranch,
                TargetBranch = project.DefaultBranch,
            });
        });

        Assert.That(exception.ErrorMessage, Is.EqualTo("[\"You can't use same project/branch for source and target\"]"));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_delete()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        mergeRequestClient.Delete(mergeRequest.Iid);

        Assert.Throws<GitLabException>(() =>
        {
            _ = mergeRequestClient[mergeRequest.Iid];
        });
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_approvers()
    {
        using var context = await GitLabTestContext.CreateAsync();

        // https://about.gitlab.com/releases/2021/04/22/gitlab-13-11-released/#removal-of-merge-request-approvers-endpoint-in-favor-of-approval-rules-api
        context.ReportTestAsInconclusiveIfGitLabVersionOutOfRange(VersionRange.Parse("[,13.11)"));

        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        var approvalClient = mergeRequestClient.ApprovalClient(mergeRequest.Iid);
        var approvers = approvalClient.Approvals.Approvers;

        Assert.That(approvers, Is.Empty, "Initially no approver defined");

        // --- Add the exampleAdminUser as approver for this MR since adding the MR owners won't increment the number of approvers---
        var userId = context.AdminClient.Users.Current.Id;

        var approversChange = new MergeRequestApproversChange
        {
            Approvers = new[] { userId },
        };

        approvalClient.ChangeApprovers(approversChange);

        approvers = approvalClient.Approvals.Approvers;

        Assert.That(approvers, Has.Length.EqualTo(1), "A single approver defined");
        Assert.That(approvers[0].User.Id, Is.EqualTo(userId), "The approver is the current user");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_unassigned_merge_requests()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        var mergeRequests = mergeRequestClient.Get(new MergeRequestQuery { AssigneeId = QueryAssigneeId.None }).ToList();
        Assert.That(mergeRequests, Has.Count.EqualTo(1), "The query retrieved all open merged requests that are unassigned");

        mergeRequests = mergeRequestClient.Get(new MergeRequestQuery { AssigneeId = context.Client.Users.Current.Id }).ToList();
        Assert.That(mergeRequests, Is.Empty, "The query retrieved all open merged requests that are unassigned");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_assigned_merge_requests()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var userId = context.Client.Users.Current.Id;
        mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate { AssigneeId = userId });

        var mergeRequests = mergeRequestClient.Get(new MergeRequestQuery { AssigneeId = QueryAssigneeId.None }).ToList();
        Assert.That(mergeRequests, Is.Empty, "The query retrieved all open merged requests that are unassigned");

        mergeRequests = mergeRequestClient.Get(new MergeRequestQuery { AssigneeId = userId }).ToList();
        Assert.That(mergeRequests, Has.Count.EqualTo(1), "The query retrieved all open merged requests that are unassigned");
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_set_reviewers_merge_requests()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        context.CreateMergeRequest(); // Second MR to verify filter returns only one
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);
        var userId = context.Client.Users.Current.Id;
        mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate { ReviewerIds = new[] { userId } });

        var mergeRequests = mergeRequestClient.Get(new MergeRequestQuery { ReviewerId = userId }).ToList();
        Assert.That(mergeRequests, Has.Count.EqualTo(1), "The query retrieved all open merged requests that are assigned for a reviewer");

        var mergeRequestUpdated = mergeRequests.Single();
        var reviewers = mergeRequestUpdated.Reviewers;
        Assert.That(reviewers, Has.Length.EqualTo(1));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_cancel_merge_when_pipeline_succeeds()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        mergeRequest.MergeWhenPipelineSucceeds = true;

        AcceptAndCancelMergeRequest(mergeRequestClient, mergeRequest);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_versions()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        var versions = await GitLabTestContext.RetryUntilAsync(
            () => mergeRequestClient.GetVersionsAsync(mergeRequest.Iid),
            versions => versions.Any(),
            TimeSpan.FromSeconds(10));

        var version = versions.First();

        Assert.That(version.HeadCommitSha, Is.EqualTo(mergeRequest.Sha));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_merge_request_head_pipeline()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var sourceProjectId = await context.Client.Projects.GetByIdAsync(mergeRequest.SourceProjectId, new SingleProjectQuery());
        JobTests.AddGitLabCiFile(context.Client, sourceProjectId, branch: mergeRequest.SourceBranch);
        var mergeRequestClient = context.Client.GetMergeRequest(project.Id);

        mergeRequest = await GitLabTestContext.RetryUntilAsync(() => mergeRequestClient[mergeRequest.Iid], p => p.HeadPipeline != null, TimeSpan.FromSeconds(120));

        Assert.That(mergeRequest.HeadPipeline?.ProjectId, Is.EqualTo(project.Id));
    }

    private static void ListMergeRequest(IMergeRequestClient mergeRequestClient, MergeRequest mergeRequest)
    {
        Assert.That(mergeRequestClient.All.Any(x => x.Id == mergeRequest.Id), Is.True, "Test 'All' accessor returns the merge request");
        Assert.That(mergeRequestClient.All.Any(x => x.DivergedCommitsCount.HasValue), Is.False, "Listing multiple MRs will not set their DivergedCommitsCount property");
        Assert.That(mergeRequestClient.AllInState(MergeRequestState.opened).Any(x => x.Id == mergeRequest.Id), Is.True, "Can return all open requests");
        Assert.That(mergeRequestClient.AllInState(MergeRequestState.merged).Any(x => x.Id == mergeRequest.Id), Is.False, "Can return all closed requests");
    }

    public static MergeRequest UpdateMergeRequest(IMergeRequestClient mergeRequestClient, MergeRequest request)
    {
        var updatedMergeRequest = mergeRequestClient.Update(request.Iid, new MergeRequestUpdate
        {
            Title = "New title",
            Description = "New description",
            Labels = "a,b",
            SourceBranch = "my-super-feature",
            TargetBranch = request.TargetBranch,
        });

        Assert.That(updatedMergeRequest.Title, Is.EqualTo("New title"));
        Assert.That(updatedMergeRequest.Description, Is.EqualTo("New description"));
        Assert.That(updatedMergeRequest.MergeWhenPipelineSucceeds, Is.False);
        Assert.That(updatedMergeRequest.Labels, Is.EqualTo(new[] { "a", "b" }).AsCollection);

        return updatedMergeRequest;
    }

    private static void Test_can_update_a_subset_of_merge_request_fields(IMergeRequestClient mergeRequestClient, MergeRequest mergeRequest)
    {
        var updated = mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate
        {
            Title = "Second update",
        });

        Assert.That(updated.Title, Is.EqualTo("Second update"));
        Assert.That(updated.Description, Is.EqualTo(mergeRequest.Description));
    }

    private static void Test_can_update_labels_with_delta(IMergeRequestClient mergeRequestClient, MergeRequest mergeRequest)
    {
        // Ensure original labels are "a,b"
        Assert.That(mergeRequest.Labels, Is.EqualTo(new[] { "a", "b" }).AsCollection);

        var updated = mergeRequestClient.Update(mergeRequest.Iid, new MergeRequestUpdate
        {
            RemoveLabels = "b",
            AddLabels = "c,d",
        });

        Assert.That(updated.Labels, Is.EqualTo(new[] { "a", "c", "d" }).AsCollection);
    }

    public static void AcceptMergeRequest(IMergeRequestClient mergeRequestClient, MergeRequest request)
    {
        Policy
            .Handle<GitLabException>(ex => ex.StatusCode is HttpStatusCode.MethodNotAllowed or HttpStatusCode.NotAcceptable)
            .Retry(10)
            .Execute(() =>
            {
                var mergeRequest = mergeRequestClient.Accept(
                mergeRequestIid: request.Iid,
                message: new MergeRequestMerge
                {
                    MergeCommitMessage = $"Merge my-super-feature into {request.TargetBranch}",
                    ShouldRemoveSourceBranch = true,
                    MergeWhenPipelineSucceeds = false,
                    Squash = false,
                });

                Assert.That(mergeRequest.State, Is.EqualTo(nameof(MergeRequestState.merged)));
            });
    }

    public static void RebaseMergeRequest(IMergeRequestClient mergeRequestClient, MergeRequest mergeRequest)
    {
        var rebaseResult = mergeRequestClient.Rebase(mergeRequestIid: mergeRequest.Iid);
        Assert.That(rebaseResult.RebaseInProgress, Is.True);
    }

    public static void AcceptAndCancelMergeRequest(IMergeRequestClient mergeRequestClient, MergeRequest request)
    {
        Policy
            .Handle<GitLabException>(ex => ex.StatusCode is HttpStatusCode.MethodNotAllowed or HttpStatusCode.NotAcceptable)
            .Retry(10)
            .Execute(() =>
            {
                Assert.That(request.MergeWhenPipelineSucceeds, Is.EqualTo(true));
                request = mergeRequestClient.CancelMergeWhenPipelineSucceeds(mergeRequestIid: request.Iid);
                Assert.That(request.MergeWhenPipelineSucceeds, Is.EqualTo(false));
            });
    }
}
