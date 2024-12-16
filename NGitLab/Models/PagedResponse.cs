using System.Collections.Generic;

namespace NGitLab.Models;

public static class PagedResponse
{
    /// <summary>
    /// The maximum number of records in a paged query before GitLab ceases to return
    /// some pagination headers such as <pre>x-total</pre>.
    /// </summary>
    /// <remarks>
    /// For performance reasons, if a query returns more than 10,000 records, GitLab
    /// excludes some headers. See https://docs.gitlab.com/ee/api/rest/index.html#pagination-response-headers.
    /// </remarks>
    internal const int MaxQueryCountLimit = 10_000;
}

public record PagedResponse<T>(IReadOnlyCollection<T> Page, int? Total);
