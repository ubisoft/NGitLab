using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IMergeRequestChangesClient {
        MergeRequestChanges Changes { get; }
    }
}
