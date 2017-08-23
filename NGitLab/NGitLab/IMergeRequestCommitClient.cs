using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IMergeRequestCommitClient {
        IEnumerable<Commit> All();
    }
}