using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab;

public interface IDeploymentClient
{
    /// <summary>
    /// Return all project's deployments
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="query">Filtering and ordering query</param>
    /// <returns></returns>
    IEnumerable<Deployment> Get(long projectId, DeploymentQuery query);

    /// <summary>
    /// Return a deployment's associated Merge Requests
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="deploymentId">Deployment ID</param>
    /// <returns></returns>
    IEnumerable<MergeRequest> GetMergeRequests(long projectId, long deploymentId);
}
