using System;
using System.Threading.Tasks;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestApprovalClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task ApproveMergeRequest_and_UnapproveMergeRequest_roundtrip()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = await context.CreateMergeRequestAsync();
        var mrClient = context.Client.GetMergeRequest(project.Id);
        var approvalClient = mrClient.ApprovalClient(mergeRequest.Iid);

        // Approve
        var approvals = approvalClient.ApproveMergeRequest();
        Assert.That(approvals, Is.Not.Null);
        Assert.That(approvals.Approved, Is.True, "MR should be marked as approved after ApproveMergeRequest");

        // Unapprove — should not throw
        Assert.DoesNotThrow((Action)(() => approvalClient.UnapproveMergeRequest()));

        // After unapproval the approval state should show no approved-by entries
        var state = approvalClient.Approvals;
        Assert.That(state.ApprovedBy, Is.Empty.Or.Null, "No approvers should remain after unapproving");
    }

    [Test]
    [NGitLabRetry]
    public async Task UnapproveMergeRequest_on_unapproved_mr_throws()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = await context.CreateMergeRequestAsync();
        var approvalClient = context.Client.GetMergeRequest(project.Id).ApprovalClient(mergeRequest.Iid);

        // Act/Assert
        Assert.That((Action)(() => approvalClient.UnapproveMergeRequest()), Throws.TypeOf<GitLabException>(),
            "Unapproving an unapproved MR should throw a GitLabException");
    }
}
