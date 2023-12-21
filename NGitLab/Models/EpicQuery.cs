using System;

namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting deployments
/// </summary>
public class EpicQuery
{
    /// <summary>
    /// Return all issues or just those that are open, closed
    /// </summary>
    public EpicState? State { get; set; }

    /// <summary>
    /// Return requests ordered by id, iid, created_at or updated_at fields. Default is id
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Return requests sorted in asc or desc order. Default is asc
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// Return epics matching a comma separated list of labels
    /// </summary>
    public string Labels { get; set; }

    /// <summary>
    /// Return epics created on or after the given time
    /// </summary>
    public DateTime? CreatedAfter { get; set; }

    /// <summary>
    /// Return epics created on or before the given time
    /// </summary>
    public DateTime? CreatedBefore { get; set; }

    /// <summary>
    /// Return epics updated on or after the given time
    /// </summary>
    public DateTime? UpdatedAfter { get; set; }

    /// <summary>
    /// Return epics updated on or before the given time
    /// </summary>
    public DateTime? UpdatedBefore { get; set; }

    /// <summary>
    /// Search issues against their title and description
    /// </summary>
    public string Search { get; set; }
}
