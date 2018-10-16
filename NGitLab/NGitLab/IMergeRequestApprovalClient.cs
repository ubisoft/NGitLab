using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestApprovalClient
    {
        MergeRequestApprovals Approvals { get; }

        void ChangeApprovers(MergeRequestApproversChange approversChange);
    }
}