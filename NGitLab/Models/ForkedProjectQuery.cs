namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting projects.
/// </summary>
public class ForkedProjectQuery
{
    /// <summary>
    /// Limit by archived status
    /// </summary>
    public bool? Archived { get; set; }

    /// <summary>
    /// Limit by visibility public, internal, or private
    /// </summary>
    public VisibilityLevel? Visibility { get; set; }

    /// <summary>
    /// Return projects ordered by id, name, path, created_at, updated_at, or last_activity_at fields. Default is created_at
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Return list of authorized projects matching the search criteria
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Return only the ID, URL, name, and path of each project
    /// </summary>
    public bool? Simple { get; set; }

    /// <summary>
    /// Include project statistics
    /// </summary>
    public bool? Statistics { get; set; }

    /// <summary>
    /// Limit by projects explicitly owned by the current user
    /// </summary>
    public bool? Owned { get; set; }

    /// <summary>
    /// Limit by projects that the current user is a member of
    /// </summary>
    public bool? Membership { get; set; }

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Limit to projects where current user has at least this access level
    /// (optional)
    /// </summary>
    public AccessLevel? MinAccessLevel { get; set; }
}
