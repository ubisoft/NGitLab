using NGitLab.Models;

namespace NGitLab;

public interface IProtectedBranchClient
{
    ProtectedBranch ProtectBranch(BranchProtect branchProtect);

    void UnprotectBranch(string branchName);

    ProtectedBranch GetProtectedBranch(string branchName);

    ProtectedBranch[] GetProtectedBranches(string search = null);
}
