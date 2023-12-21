using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ProtectedBranch : GitLabObject
{
    public long Id { get; set; }

    public string Name { get; set; }

    public AccessLevelInfo[] PushAccessLevels { get; set; }

    public AccessLevelInfo[] MergeAccessLevels { get; set; }

    public bool AllowForcePush { get; set; }

    public bool CodeOwnerApprovalRequired { get; set; }

    internal Models.ProtectedBranch ToProtectedBranchClient()
    {
        return new Models.ProtectedBranch
        {
            Id = Id,
            Name = Name,
            PushAccessLevels = PushAccessLevels,
            MergeAccessLevels = MergeAccessLevels,
            AllowForcePush = AllowForcePush,
            CodeOwnerApprovalRequired = CodeOwnerApprovalRequired,
        };
    }
}
