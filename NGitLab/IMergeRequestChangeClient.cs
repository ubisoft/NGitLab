using NGitLab.Models;

namespace NGitLab;

public interface IMergeRequestChangeClient
{
    MergeRequestChange MergeRequestChange { get; }
}
