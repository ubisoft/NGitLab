using NGitLab.Models;

namespace NGitLab;

public interface IMergeRequestApprovalClient
{
    MergeRequestApprovals Approvals { get; }

    void ChangeApprovers(MergeRequestApproversChange approversChange);

    MergeRequestApprovals ApproveMergeRequest(MergeRequestApproveRequest request = null);

    void UnapproveMergeRequest();

    /// <summary>
    /// Available only for bot users based on project or group tokens.
    /// </summary>
    void ResetApprovals();

    MergeRequestApprovalState GetApprovalState();
}
