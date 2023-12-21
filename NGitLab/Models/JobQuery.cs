namespace NGitLab.Models;

/// <summary>
/// Allows fine-grained control on queried jobs
/// </summary>
public sealed class JobQuery
{
    /// <summary>
    /// Specifies the scope of jobs to show: Created, Pending, Running, ...
    /// </summary>
    public JobScopeMask Scope { get; set; }

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }
}
