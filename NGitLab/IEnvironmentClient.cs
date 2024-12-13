using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IEnvironmentClient
{
    /// <summary>
    /// All the environment of the project
    /// </summary>
    IEnumerable<EnvironmentInfo> All { get; }

    /// <summary>
    /// Create a new environment in the project
    /// </summary>
    /// <param name="name">Name of the new environment</param>
    /// <param name="externalUrl">Place to link to for this environment (can be null)</param>
    /// <returns>The newly created environment</returns>
    EnvironmentInfo Create(string name, string externalUrl);

    /// <summary>
    /// Updates an existing environment's name and/or external_url.
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    /// <param name="name">The new name of the environment</param>
    /// <param name="externalUrl">The new external url</param>
    /// <returns>The updated environment</returns>
    /// <remarks>
    /// - Renaming an environment by using the API was deprecated in GitLab 15.9.
    /// - Renaming an environment with the API removed in GitLab 16.0.
    /// </remarks>
    [EditorBrowsable(EditorBrowsableState.Never)]
    EnvironmentInfo Edit(long environmentId, string name, string externalUrl);

    /// <summary>
    /// Updates an existing environment's external_url.
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    /// <param name="externalUrl">The new external url</param>
    /// <returns>The updated environment</returns>
    EnvironmentInfo Edit(long environmentId, string externalUrl);

    /// <summary>
    /// Delete an environment.
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    void Delete(long environmentId);

    /// <summary>
    /// Stop an environment.
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    /// <returns>The stopped environment</returns>
    EnvironmentInfo Stop(long environmentId);

    /// <summary>
    /// List environments of the project
    /// </summary>
    /// <param name="query"><see cref="EnvironmentQuery"></see>/></param>
    /// <returns>Environments that matched the query criteria</returns>
    GitLabCollectionResponse<EnvironmentInfo> GetEnvironmentsAsync(EnvironmentQuery query);

    /// <summary>
    /// Get a specific environment
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    /// <returns>The environment with the corresponding ID</returns>
    EnvironmentInfo GetById(long environmentId);

    /// <summary>
    /// Get a specific environment
    /// </summary>
    /// <param name="environmentId">The ID of the environment</param>
    /// <returns>The environment with the corresponding ID</returns>
    Task<EnvironmentInfo> GetByIdAsync(long environmentId, CancellationToken cancellationToken = default);
}
