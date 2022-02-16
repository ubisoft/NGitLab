using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IIssueClient
    {
        /// <summary>
        /// Get a list of all project issues
        /// </summary>
        IEnumerable<Issue> Owned { get; }

        /// <summary>
        /// Get a list of issues for the specified project.
        /// </summary>
        IEnumerable<Issue> ForProject(int projectId);

        GitLabCollectionResponse<Issue> ForProjectAsync(int projectId);

        /// <summary>
        ///     <para>Return a single issue for a project given project.</para>
        ///     <para>url like GET /projects/:id/issues/:issue_id</para>
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="issueId"></param>
        /// <returns>The issue that corresponds to the project id and issue id</returns>
        Issue Get(int projectId, int issueId);

        Task<Issue> GetAsync(int projectId, int issueId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Return issues for a given query.
        ///
        /// url like GET /issues?<parameter_name>=<value>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns>the query's related issues</returns>
        IEnumerable<Issue> Get(IssueQuery query);

        GitLabCollectionResponse<Issue> GetAsync(IssueQuery query);

        /// <summary>
        /// Return project issues for a given query.
        ///
        /// url like GET /projects/:id/issues?<parameter_name>=<value>
        ///
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="query"></param>
        /// <returns>the query's related issues</returns>
        IEnumerable<Issue> Get(int projectId, IssueQuery query);

        GitLabCollectionResponse<Issue> GetAsync(int projectId, IssueQuery query);

        /// <summary>
        /// Add an issue witht he proposed title to the GitLab list for the selected proejct id.
        /// </summary>
        /// <param name="issueCreate"></param>
        /// <returns>The issue if it was created.  Null if not.</returns>
        Issue Create(IssueCreate issueCreate);

        Task<Issue> CreateAsync(IssueCreate issueCreate, CancellationToken cancellationToken = default);

        /// <summary>
        /// Edit and save an issue.
        /// </summary>
        /// <param name="issueEdit"></param>
        /// <returns>The issue if it's updated.  Null if not.</returns>
        Issue Edit(IssueEdit issueEdit);

        Task<Issue> EditAsync(IssueEdit issueEdit, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the resource label events.
        ///
        /// url like GET /projects/:id/issues/:issue_iid/resource_label_events
        ///
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="issueIid">The id of the issue in the project's scope.</param>
        /// <returns>The issue if it's updated.  Null if not.</returns>
        IEnumerable<ResourceLabelEvent> ResourceLabelEvents(int projectId, int issueIid);

        GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(int projectId, int issueIid);

        /// <summary>
        /// Get all merge requests that are related to a particular issue.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="issueIid">The id of the issue in the project's scope.</param>
        /// <returns>The list of MR that are related this issue.</returns>
        IEnumerable<MergeRequest> RelatedTo(int projectId, int issueIid);

        GitLabCollectionResponse<MergeRequest> RelatedToAsync(int projectId, int issueIid);

        /// <summary>
        /// Get all merge requests that close a particular issue when merged.
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="issueIid">The id of the issue in the project's scope.</param>
        /// <returns>The list of MR that closed this issue.</returns>
        IEnumerable<MergeRequest> ClosedBy(int projectId, int issueIid);

        GitLabCollectionResponse<MergeRequest> ClosedByAsync(int projectId, int issueIid);

        /// <summary>
        /// Get time tracking statistics
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="issueIid">The id of the issue in the project's scope.</param>
        /// <returns>The time tracking statistics of the issue.</returns>
        Task<TimeStats> TimeStatsAsync(int projectId, int issueIid, CancellationToken cancellationToken = default);
    }
}
