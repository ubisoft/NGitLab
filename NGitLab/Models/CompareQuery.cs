namespace NGitLab.Models;

/// <summary>
/// Query details for comparison of branches/tags/commit hashes
/// </summary>
public class CompareQuery
{
    /// <summary>
    /// The source for comparison, can be a branch, tag or a commit hash.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// The target for comparison, can be a branch, tag or a commit hash.
    /// </summary>
    public string Target { get; set; }

    public CompareQuery(string source, string target)
    {
        Source = source;
        Target = target;
    }
}
