namespace NGitLab;

public sealed class SearchQuery
{
    /// <summary>
    /// The search query
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Filter by state. Issues and merge requests are supported; it is ignored for other scopes.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Filter by confidentiality. Issues scope is supported; it is ignored for other scopes.
    /// </summary>
    public bool? Confidential { get; set; }

    /// <summary>
    /// Allowed values are created_at only. If this is not set, the results are either sorted by
    /// created_at in descending order for basic search, or by the most relevant documents when using advanced search.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Allowed values are asc or desc only. If this is not set, the results are either sorted
    /// by created_at in descending order for basic search, or by the most relevant documents when using advanced search.
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Specifies the start page.
    /// </summary>
    public int? Page { get; set; }
}
