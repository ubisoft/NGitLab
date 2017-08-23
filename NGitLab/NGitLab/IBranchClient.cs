using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IBranchClient {
        IEnumerable<Branch> All();
        Branch Get(string name);
        Branch Protect(string name);
        Branch Unprotect(string name);
        Branch Create(BranchCreate branch);
        BranchRemovalStatus Delete(string name);
    }
}