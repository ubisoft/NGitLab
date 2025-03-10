using NGitLab.Models;

namespace NGitLab.Impl;

public interface IGitDiffChangeCounter
{
    DiffStats Compute(MergeRequestChange mergeRequestChange);
}
