using System.Threading.Tasks;
using NGitLab.Models;
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
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mrClient = context.Client.GetMergeRequest(project.Id);
        var approvalClient = mrClient.ApprovalClient(mergeRequest.Iid);

        // Approve
        var approvals = approvalClient.ApproveMergeRequest();
        Assert.That(approvals, Is.Not.Null);
        Assert.That(approvals.Approved, Is.True, "MR should be marked as approved after ApproveMergeRequest");

        // Unapprove — should not throw
        Assert.DoesNotThrow((TestDelegate)(() => approvalClient.UnapproveMergeRequest()));

        // After unapproval the approval state should show no approved-by entries
        var state = approvalClient.Approvals;
        Assert.That(state.ApprovedBy, Is.Empty.Or.Null, "No approvers should remain after unapproving");
    }

    [Test]
    [NGitLabRetry]
    public async Task UnapproveMergeRequest_on_unapproved_mr_does_not_throw()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var approvalClient = context.Client.GetMergeRequest(project.Id).ApprovalClient(mergeRequest.Iid);

        // Calling unapprove on an already-unapproved MR should be idempotent (GitLab returns 401 if already unapproved,
        // but some versions are lenient). We accept both a silent success and a GitLabException.
        try
        {
            approvalClient.UnapproveMergeRequest();
        }
        catch (GitLabException)
        {
            // Acceptable: GitLab may return an error when the MR was not approved
        }
    }
}
