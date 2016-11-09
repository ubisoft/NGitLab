using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    /// <summary>
    /// Uses: https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/runners.md
    /// </summary>
    public interface IRunnerClient
    {
        /// <summary>
        /// Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Runner> Accessible { get; }
        
        /// <summary>
        /// Get a list of all GitLab projects (admin only).
        /// </summary>
        IEnumerable<Runner> All { get; }

        /// <summary>
        /// Get details of a runner
        /// </summary>
        Runner this[int id] { get; }

        /// <summary>
        /// Deletes the specified runner.
        /// </summary>
        void Delete(Runner runner);

        /// <summary>
        /// Deletes the specified runner.
        /// </summary>
        void Delete(int runnerId);
    }
}