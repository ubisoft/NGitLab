namespace NGitLab.Models;

/// <summary>
/// Query parameters for listing statuses of a commit. Refer to
/// <see href="https://docs.gitlab.com/ee/api/commits.html#list-the-statuses-of-a-commit">GitLab's documentation</see>
/// for further details.
/// </summary>
public sealed class CommitStatusQuery
{
    /// <summary>
    /// Name of the branch or tag. Default is the default branch.
    /// </summary>
    public string Ref { get; init; }

    /// <summary>
    /// Filter statuses by build stage. For example, 'test'.
    /// </summary>
    public string Stage { get; init; }

    /// <summary>
    /// Filter statuses by job name. For example, 'bundler:audit'.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Filter statuses by pipeline ID. For example, '1234'.
    /// </summary>
    public long? PipelineId { get; init; }

    /// <summary>
    /// Values for sorting statuses. Valid values are 'id' and 'pipeline_id'. Default is 'id'.
    /// </summary>
    public string OrderBy { get; init; }

    /// <summary>
    /// Sort statuses in ascending or descending order. Valid values are 'asc' and 'desc'. Default is 'asc'.
    /// </summary>
    public string Sort { get; init; }

    /// <summary>
    /// Include all statuses instead of latest only. Default is 'false'.
    /// </summary>
    public bool? All { get; init; }

    /// <summary>
    /// Number of items to return by page.
    /// </summary>
    public int? PerPage { get; init; }
}
