using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IProjectClient
    {
        /// <summary>
        /// Get a list of projects for which the authenticated user is a member.
        /// </summary>
        IEnumerable<Project> Accessible { get; }

        /// <summary>
        /// Get a list of projects owned by the authenticated user.
        /// </summary>
        IEnumerable<Project> Owned { get; }

        /// <summary>
        /// Get a list of projects which the authenticated user can see.
        /// </summary>
        IEnumerable<Project> Visible { get; }

        /// <summary>
        /// Get a list of all GitLab projects (admin only).
        /// </summary>
        IEnumerable<Project> All { get; }

        Project this[int id] { get; }

        /// <summary>
        /// Returns the project with the provided full name in the form Namespace/Name.
        /// </summary>
        /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/projects.md#get-single-project</remarks>
        Project this[string fullName] { get; }

        Project Create(ProjectCreate project);
        
        bool Delete(int id);
    }
}