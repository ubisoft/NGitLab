namespace NGitLab.Models;

public class ReleaseQuery
{
    /// <summary>
    /// The field to use as order. Either released_at (default) or created_at.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// The direction of the order. Either desc (default) for descending order or asc for ascending order.
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// If true, a response includes HTML rendered Markdown of the release description.
    /// </summary>
    public bool? IncludeHtmlDescription { get; set; }

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Specifies the start page.
    /// </summary>
    public int? Page { get; set; }
}
