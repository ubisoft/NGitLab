using System;
using System.Collections.Generic;

namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting projects.
/// </summary>
public class ProjectQuery
{
    public ProjectQueryScope Scope = ProjectQueryScope.Accessible;

    /// <summary>
    /// Limit by archived status
    /// </summary>
    public bool? Archived;

    /// <summary>
    /// Limit by visibility public, internal, or private
    /// </summary>
    public VisibilityLevel? Visibility;

    /// <summary>
    /// Return projects ordered by id, name, path, created_at, updated_at, or last_activity_at fields. Default is created_at
    /// </summary>
    public string OrderBy;

    /// <summary>
    /// Return list of authorized projects matching the search criteria
    /// </summary>
    public string Search;

    /// <summary>
    /// Order projects results ascending. Default is descending
    /// </summary>
    public bool? Ascending;

    /// <summary>
    /// Return only the ID, URL, name, and path of each project
    /// </summary>
    public bool? Simple;

    /// <summary>
    /// Include project statistics
    /// </summary>
    public bool? Statistics;

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage;

    /// <summary>
    /// Project visible by user
    /// </summary>
    public int? UserId;

    /// <summary>
    /// Limit to projects where current user has at least this access level
    /// (optional)
    /// </summary>
    public AccessLevel? MinAccessLevel;

    /// <summary>
    /// Limit results to projects with last_activity after specified time.
    /// </summary>
    public DateTimeOffset? LastActivityAfter;

    /// <summary>
    /// Limit results to projects that match all of the given topics.
    /// </summary>
    public IList<string> Topics { get; } = new List<string>();
}

public enum ProjectQueryScope
{
    /// <summary>
    /// Get a list of projects for which the authenticated user is a member.
    /// </summary>
    Accessible,

    /// <summary>
    /// Get a list of projects owned by the authenticated user.
    /// </summary>
    Owned,

    /// <summary>
    /// Get a list of projects which the authenticated user can see.
    /// </summary>
    [Obsolete("Use All instead which is the same behavior.")]
    Visible,

    /// <summary>
    /// Get a list of all GitLab projects.
    /// </summary>
    All,
}
