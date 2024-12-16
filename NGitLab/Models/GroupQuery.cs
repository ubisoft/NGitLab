namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting groups (based on v4 GitLab API).
/// https://docs.gitlab.com/ee/api/groups.html
/// </summary>
public class GroupQuery
{
    /// <summary>
    /// Skip the group IDs passed
    /// (optional)
    /// </summary>
    public long[] SkipGroups { get; set; }

    /// <summary>
    /// Show all the groups you have access to (defaults to false for authenticated users, true for admin); Attributes owned and min_access_level have precedence
    /// (optional)
    /// </summary>
    public bool? AllAvailable { get; set; }

    /// <summary>
    /// Return the list of authorized groups matching the search criteria
    /// (optional)
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Order groups by name, path or id. Default is name
    /// (optional)
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Order groups in asc or desc order. Default is ascending
    /// (optional)
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Include group statistics (admins only)
    /// (optional)
    /// </summary>
    public bool? Statistics { get; set; }

    /// <summary>
    /// Include custom attributes in response (admins only)
    /// (optional)
    /// </summary>
    public bool? WithCustomAttributes { get; set; }

    /// <summary>
    /// Limit to groups explicitly owned by the current user
    /// (optional)
    /// </summary>
    public bool? Owned { get; set; }

    /// <summary>
    /// Limit to groups where current user has at least this access level
    /// (optional)
    /// </summary>
    public AccessLevel? MinAccessLevel { get; set; }

    /// <summary>
    /// Limit to top level groups, excluding all subgroups. Default is false.
    /// (optional)
    /// </summary>
    public bool? TopLevelOnly { get; set; }
}
