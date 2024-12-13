using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl;

/// <summary>
/// Manages approvals on a specific Merge Request
/// </summary>
/// <see cref="https://docs.gitlab.com/ee/api/merge_request_approvals.html#merge-request-level-mr-approvals"/>
public class MergeRequestApprovalClient : IMergeRequestApprovalClient
{
    private readonly API _api;
    private readonly string _approvalsPath;
    private readonly string _approversPath;
    private readonly string _approvePath;
    private readonly string _resetApprovalsPath;

    public MergeRequestApprovalClient(API api, string projectPath, long mergeRequestIid)
    {
        var iid = mergeRequestIid.ToString(CultureInfo.InvariantCulture);
        _api = api;
        _approvalsPath = projectPath + "/merge_requests/" + iid + "/approvals";
        _approversPath = projectPath + "/merge_requests/" + iid + "/approvers";
        _approvePath = projectPath + "/merge_requests/" + iid + "/approve";
        _resetApprovalsPath = projectPath + "/merge_requests/" + iid + "/reset_approvals";
    }

    public MergeRequestApprovals Approvals => _api.Get().To<MergeRequestApprovals>(_approvalsPath);

    public MergeRequestApprovals ApproveMergeRequest(MergeRequestApproveRequest request = null)
        => _api.Post().With(request ?? new MergeRequestApproveRequest()).To<MergeRequestApprovals>(_approvePath);

    public void ChangeApprovers(MergeRequestApproversChange approversChange) => _api.Put().With(approversChange).To<MergeRequestApproversChange>(_approversPath);

    public void ResetApprovals() => _api.Put().Execute(_resetApprovalsPath);
}
