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
        IEnumerable<Project> Get(ProjectQuery query);

        Project this[int id] { get; }

        /// <summary>
        /// Returns the project with the provided full name in the form Namespace/Name.
        /// </summary>
        /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/projects.md#get-single-project</remarks>
        Project this[string fullName] { get; }

        Project Create(ProjectCreate project);

        void Delete(int id);

        Project GetById(int id, SingleProjectQuery query);

        Project Fork(string id, ForkProject forkProject);

        IEnumerable<Project> GetForks(string id, ForkedProjectQuery query);

        Dictionary<string, double> GetLanguages(string id);
    }
}
