using System;
using System.Collections.Generic;
using NGitLab.Models;
using Environment = NGitLab.Models.Environment;

namespace NGitLab
{
    public interface IEnvironmentClient
    {
        /// <summary>
        /// All the environment of the project
        /// </summary>
        IEnumerable<Environment> All { get; }

        /// <summary>
        /// Create a new environment in the project
        /// </summary>
        /// <param name="name">Name of the new environment</param>
        /// <param name="externalUrl">Place to link to for this environment (can be null)</param>
        /// <returns>The newly created environment</returns>
        Environment Create(string name, string externalUrl);

        /// <summary>
        /// Updates an existing environment's name and/or external_url.
        /// </summary>
        /// <param name="environmentId">The ID of the environment</param>
        /// <param name="name">The new name of the environment</param>
        /// <param name="externalUrl">The new external url</param>
        /// <returns>The updated environment</returns>
        Environment Edit(int environmentId, string name, string externalUrl);

        /// <summary>
        /// Delete an environment.
        /// </summary>
        /// <param name="environmentId">The ID of the environment</param>
        void Delete(int environmentId);

        /// <summary>
        /// Stop an environment.
        /// </summary>
        /// <param name="environmentId">The ID of the environment</param>
        /// <returns>The stopped environment</returns>
        Environment Stop(int environmentId);
    }
}
