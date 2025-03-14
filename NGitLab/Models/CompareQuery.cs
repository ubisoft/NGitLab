using System.Text.Json.Serialization;

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

    /// <summary>
    /// Comparison method: true for direct comparison between from and to (from..to), false to compare using merge base (from…to)’. Default is false.
    /// </summary>
    public bool? Straight { get; set; }

    /// <summary>
    /// Present diffs in the unified diff format https://www.gnu.org/software/diffutils/manual/html_node/Detailed-Unified.html. Default is false. Introduced in GitLab 16.5.
    /// </summary>
    public bool? Unidiff { get; set; }

    /// <summary>
    /// The ID to compare from.
    /// </summary>
    public long? FromProjectId { get; set; }

    public CompareQuery(string source, string target)
    {
        Source = source;
        Target = target;
    }
}
