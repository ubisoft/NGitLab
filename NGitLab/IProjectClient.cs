using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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

        GitLabCollectionResponse<Project> GetAsync(ProjectQuery query);

        Project this[int id] { get; }

        /// <summary>
        /// Returns the project with the provided full name in the form Namespace/Name.
        /// </summary>
        /// <remarks>https://github.com/gitlabhq/gitlabhq/blob/master/doc/api/projects.md#get-single-project</remarks>
        Project this[string fullName] { get; }

        Project Create(ProjectCreate project);

        Project Update(string id, ProjectUpdate projectUpdate);

        void Delete(int id);

        void Archive(int id);

        void Unarchive(int id);

        /// <summary>
        /// Uploads a file to the specified project to be used in an issue or merge request description, or a comment.
        /// </summary>
        UploadedProjectFile UploadFile(string id, FormDataContent data);

        Project GetById(int id, SingleProjectQuery query);

        Task<Project> GetByIdAsync(int id, SingleProjectQuery query, CancellationToken cancellationToken = default);

        Task<Project> GetByNamespacedPathAsync(string path, SingleProjectQuery query = null, CancellationToken cancellationToken = default);

        Project Fork(string id, ForkProject forkProject);

        Task<Project> ForkAsync(string id, ForkProject forkProject, CancellationToken cancellationToken = default);

        IEnumerable<Project> GetForks(string id, ForkedProjectQuery query);

        GitLabCollectionResponse<Project> GetForksAsync(string id, ForkedProjectQuery query);

        Dictionary<string, double> GetLanguages(string id);
    }
}
