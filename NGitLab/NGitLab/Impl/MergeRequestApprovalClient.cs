using NGitLab.Models;

namespace NGitLab.Impl
{
    /// <summary>
    /// Manages approvals on a specific Merge Request
    /// </summary>
    /// <see cref="https://docs.gitlab.com/ee/api/merge_request_approvals.html#merge-request-level-mr-approvals"/>
    public class MergeRequestApprovalClient : IMergeRequestApprovalClient
    {
        private readonly API _api;
        private readonly string _approvalsPath;
        private readonly string _approversPath;

        public MergeRequestApprovalClient(API api, string projectPath, int mergeRequestIid)
        {
            _api = api;
            _approvalsPath = projectPath + "/merge_requests/" + mergeRequestIid + "/approvals";
            _approversPath = projectPath + "/merge_requests/" + mergeRequestIid + "/approvers";
        }

        public MergeRequestApprovals Approvals => _api.Get().To<MergeRequestApprovals>(_approvalsPath);

        public void ChangeApprovers(MergeRequestApproversChange approversChange) => _api.Put().With(approversChange).To<MergeRequestApproversChange>(_approversPath);
    }
}