namespace NGitLab.Models;

public class LabelQuery
{
    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Keyword to filter labels by.
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Include ancestor groups. Defaults to true.
    /// </summary>
    public bool? IncludeAncestorGroups { get; set; }

    /// <summary>
    /// Whether or not to include issue and merge request counts. Defaults to false.
    /// </summary>
    public bool? WithCounts { get; set; }
}
