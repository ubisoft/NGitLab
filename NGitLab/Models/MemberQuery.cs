using System.Collections.Generic;

namespace NGitLab.Models;

public class MemberQuery
{
    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Filters results based on a given name, email, or username. Use partial values to widen the scope of the query.
    /// </summary>
    public string Query { get; set; }

    /// <summary>
    /// Filter the results on the given user IDs.
    /// </summary>
    public List<int> UserIds { get; set; }

    /// <summary>
    /// Filter skipped users out of the results.
    /// </summary>
    public List<int> SkipUsers { get; set; }

    /// <summary>
    /// Filter results by member state, one of awaiting or active. Premium and Ultimate only.
    /// </summary>
    public string State { get; set; }

    /// <summary>
    /// Show seat information for users.
    /// </summary>
    public bool? ShowSeatInfo { get; set; }
}
