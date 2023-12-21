using System;

namespace NGitLab.Models;

/// <summary>
/// Allows to use more advanced GitLab queries for getting deployments
/// </summary>
public class DeploymentQuery
{
    /// <summary>
    /// Return all deployments or just those that are created, running, success, failed or canceled
    /// </summary>
    public DeploymentStatus? Status { get; set; }

    /// <summary>
    /// Return requests ordered by id, iid, created_at or updated_at fields. Default is id
    /// </summary>
    public string OrderBy { get; set; }

    /// <summary>
    /// Return requests sorted in asc or desc order. Default is asc
    /// </summary>
    public string Sort { get; set; }

    /// <summary>
    /// The name of the environment to filter deployments by
    /// </summary>
    public string Environment { get; set; }

    /// <summary>
    /// Return deployments updated on or after the given time
    /// </summary>
    public DateTime? UpdatedAfter { get; set; }

    /// <summary>
    /// Return deployments updated on or before the given time
    /// </summary>
    public DateTime? UpdatedBefore { get; set; }
}
