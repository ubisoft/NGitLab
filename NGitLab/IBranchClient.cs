using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IBranchClient
{
    IEnumerable<Branch> All { get; }

    IEnumerable<Branch> Search(string search);

    GitLabCollectionResponse<Branch> SearchAsync(string search);

    Branch this[string name] { get; }

    Branch Protect(string name);

    Branch Unprotect(string name);

    Branch Create(BranchCreate branch);

    void Delete(string name);
}
