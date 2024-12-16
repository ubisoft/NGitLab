using System;
using NGitLab.Models;

namespace NGitLab;

public class UserQuery
{
    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Get users that match the search query
    /// </summary>
    public string Search { get; set; }

    /// <summary>
    /// Get users that match the username
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// If true, get only users that are active
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// If true, get only users that are blocked
    /// </summary>
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// If true, get only users that are external
    /// </summary>
    public bool? IsExternal { get; set; }

    /// <summary>
    /// Exclude external users
    /// </summary>
    public bool? ExcludeExternal { get; set; }

    /// <summary>
    /// (Admin only) Return projects ordered by id, name, username, created_at, or updated_at. Default is id.
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// (Admin only) Order users in asc or desc order. Default is descending
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// (Admin only) Search users based on external UID
    /// </summary>
    public string ExternalUid { get; set; }

    /// <summary>
    /// (Admin only) Search users based on Provider
    /// </summary>
    public string Provider { get; set; }

    /// <summary>
    /// (Admin only) Search for users without projects
    /// </summary>
    public bool? WithoutProjects { get; set; }

    /// <summary>
    /// (Admin only) Search for users created before specified date time
    /// Format: "YYYY-MM-DDThh:mm:ss.sTZD" eg. "2001-01-02T00:00:00.060Z"
    /// </summary>
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// (Admin only) Search for users created after specified date time
    /// Format: "YYYY-MM-DDThh:mm:ss.sTZD" eg. "2001-01-02T00:00:00.060Z"
    /// </summary>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// (Admin only) Include the users custom attributes in the response
    /// </summary>
    public bool? WithCustomAttributes { get; set; }

    /// <summary>
    /// (Admin only) Gets users that have two factor either enabled or disabled
    /// Possible values: "enabled", "disabled"
    /// </summary>
    public TwoFactorState? TwoFactor { get; set; }

    /// <summary>
    /// (Admin only)If true, get only users that are Admin
    /// </summary>
    public bool? IsAdmin { get; set; }
}
