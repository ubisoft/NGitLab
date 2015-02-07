using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectClient
    {
        /// <summary>
        /// Get a list of projects accessible by the authenticated user.
        /// </summary>
        IEnumerable<Project> Accessible { get; }

        /// <summary>
        /// Get a list of projects owned by the authenticated user.
        /// </summary>
        IEnumerable<Project> Owned { get; }

        /// <summary>
        /// Get a list of all GitLab projects (admin only).
        /// </summary>
        IEnumerable<Project> All { get; }

        Project this[int id] { get; }

        Project Create(ProjectCreate project);
        
        bool Delete(int id);
    }
}