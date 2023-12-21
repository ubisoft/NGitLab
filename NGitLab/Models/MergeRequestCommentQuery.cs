namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting projects.
/// </summary>
public class MergeRequestCommentQuery
{
    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Specifies the specific page desired.
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Specifies if the sorting is ascending or descending
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Specifies the field the sorting should be done on (created_at or updated_at)
    /// </summary>
    public string OrderBy { get; set; }
}
