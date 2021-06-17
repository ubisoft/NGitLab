using NGitLab.Models;

namespace NGitLab
{
    public interface IProtectedBranchesClient
    {
        ProtectedBranch ProtectBranch(BranchProtect branchProtect);

        ProtectedBranch GetProtectedBranch(string branchName);

        ProtectedBranch GetProtectedBranches(string search);
    }
}
