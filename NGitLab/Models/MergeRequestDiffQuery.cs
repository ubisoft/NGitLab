namespace NGitLab.Models;

/// <summary>
/// Query options for <c>GET /projects/:id/merge_requests/:iid/diffs</c>.
/// </summary>
public class MergeRequestDiffQuery
{
    /// <summary>
    /// Present diffs in the unified diff format. Default is <see langword="false"/>.
    /// Requires GitLab 16.5 or later.
    /// </summary>
    public bool? Unidiff { get; set; }
}
