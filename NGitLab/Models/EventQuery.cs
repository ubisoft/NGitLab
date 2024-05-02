using System;

namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting issues
/// </summary>
public class EventQuery
{
    /// <summary>
    /// Include only events of a particular action type
    /// </summary>
    public EventAction? Action { get; set; }

    /// <summary>
    /// Include only events of a particular target type
    /// </summary>
    public EventTargetType? Type { get; set; }

    /// <summary>
    /// Include only events created before a particular date.
    /// </summary>
    public DateTime? Before { get; set; }

    /// <summary>
    /// Include only events created after a particular date.
    /// </summary>
    public DateTime? After { get; set; }

    /// <summary>
    /// Include all events across a user’s projects.
    /// </summary>
    public string Scope { get; set; }

    /// <summary>
    /// Sort events in asc or desc order by created_at. Default is desc
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Specifies how many records per page (GitLab supports a maximum of 100 items per page and defaults to 20).
    /// </summary>
    public int? PerPage { get; set; }

    /// <summary>
    /// Specifies which page will be retrevied.
    /// </summary>
    public int? Page { get; set; }
}
