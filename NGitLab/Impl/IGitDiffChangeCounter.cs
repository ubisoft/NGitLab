using NGitLab.Models;

namespace NGitLab.Impl;

public interface IGitDiffChangeCounter
{
    public DiffStats Compute(MergeRequestChange mergeRequestChange);
}
