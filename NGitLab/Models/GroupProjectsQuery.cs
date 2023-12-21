namespace NGitLab.Models;

public sealed class GroupProjectsQuery
{
    /// <summary>
    /// Limit by archived status
    /// </summary>
    public bool? Archived { get; set; }

    /// <summary>
    /// Limit by visibility
    /// </summary>
    public VisibilityLevel? Visibility { get; set; }

    /// <summary>
    /// Return projects ordered by id, name, path, created_at, updated_at, similarity, or last_activity_at fields. Default is created_at
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Return projects sorted in asc or desc order. Default is desc
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Return list of authorized projects matching the search criteria
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Return only the ID, URL, name, and path of each project
    /// </summary>
    public bool? Simple { get; set; }

    /// <summary>
    /// Limit by projects owned by the current user
    /// </summary>
    public bool? Owned { get; set; }

    /// <summary>
    /// Limit by projects starred by the current user
    /// </summary>
    public bool? Starred { get; set; }

    /// <summary>
    /// Limit by projects with issues feature enabled. Default is false
    /// </summary>
    public bool? WithIssuesEnabled { get; set; }

    /// <summary>
    /// Limit by projects with merge requests feature enabled. Default is false
    /// </summary>
    public bool? WithMergeRequestsEnabled { get; set; }

    /// <summary>
    /// Include projects shared to this group. Default is true
    /// </summary>
    public bool? WithShared { get; set; }

    /// <summary>
    /// Include projects in subgroups of this group. Default is false
    /// </summary>
    public bool? IncludeSubGroups { get; set; }

    /// <summary>
    /// Limit to projects where current user has at least this access level
    /// </summary>
    public AccessLevel? MinAccessLevel { get; set; }

    /// <summary>
    /// Include custom attributes in response (administrators only)
    /// </summary>
    public bool? WithCustomAttributes { get; set; }

    /// <summary>
    /// Return only projects that have security reports artifacts present in any of their builds. This means “projects with security reports enabled”. Default is false
    /// </summary>
    public bool? WithSecurityReports { get; set; }
}
