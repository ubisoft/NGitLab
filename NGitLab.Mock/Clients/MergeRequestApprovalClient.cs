using System;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestApprovalClient : ClientBase, IMergeRequestApprovalClient
{
    private readonly long _projectId;
    private readonly long _mergeRequestIid;

    public MergeRequestApprovalClient(ClientContext context, long projectId, long mergeRequestIid)
        : base(context)
    {
        _projectId = projectId;
        _mergeRequestIid = mergeRequestIid;
    }

    public MergeRequestApprovals Approvals => new();

    public MergeRequestApprovals ApproveMergeRequest(MergeRequestApproveRequest request = null)
    {
        throw new NotImplementedException();
    }

    public void ResetApprovals()
    {
        throw new NotImplementedException();
    }

    public void ChangeApprovers(MergeRequestApproversChange approversChange)
    {
        throw new NotImplementedException();
    }
}
