using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class ProtectedBranchCollection : Collection<ProtectedBranch>
{
    public ProtectedBranchCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public ProtectedBranch GetById(long branchId)
    {
        return this.FirstOrDefault(i => i.Id == branchId);
    }

    public override void Add(ProtectedBranch branch)
    {
        if (branch is null)
            throw new ArgumentNullException(nameof(branch));

        if (branch.Id == default)
        {
            branch.Id = Server.GetNewProtectedBranchId();
        }
        else if (GetById(branch.Id) != null)
        {
            throw new GitLabException("Protected Branch already exists");
        }

        base.Add(branch);
    }
}
