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

        /// <summary>
        /// Returns the project with the provided full name in the form Namespace/Name.
        /// </summary>
        /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/projects.md#get-single-project</remarks>
        Project this[string fullName] { get; }

        /// <summary>
        /// Returns the members of a project.
        /// </summary>
        /// <param name="projectId">The id or fullname of the project.</param>
        IMembersClient GetMembers(string projectId);

        Project Create(ProjectCreate project);
        
        bool Delete(int id);
    }
}