namespace NGitLab.Models;

/// <summary>
/// A filter and sort query when <see href="https://docs.gitlab.com/ee/api/tags.html#list-all-project-repository-tags">
/// listing all project repository tags</see>.
/// </summary>
public class TagQuery
{
    /// <summary>
    /// Specifies how to order tags, i.e. by "name", "updated" or (semantic) "version". Default is "updated".
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Sort order, i.e. "asc" or "desc". Default is "desc".
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Number of results to return per page. Default is 20.
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Search criteria. You can use "^term" and "term$" to find tags that begin and end with "term". No other regular expressions are supported.
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Start page number. Default is 1.
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Previous tag name, i.e. tag to start the pagination from. Used to fetch the next set of results.
    /// </summary>
    public string PageToken { get; set; }
}
