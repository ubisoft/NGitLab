using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IIssueClient
{
    /// <summary>
    /// Get a list of all project issues
    /// </summary>
    IEnumerable<Issue> Owned { get; }

    /// <summary>
    /// Get a list of issues for the specified project.
    /// </summary>
    IEnumerable<Issue> ForProject(long projectId);

    GitLabCollectionResponse<Issue> ForProjectAsync(long projectId);

    GitLabCollectionResponse<Issue> ForGroupsAsync(long groupId);

    GitLabCollectionResponse<Issue> ForGroupsAsync(long groupId, IssueQuery query);

    /// <summary>
    ///     <para>Return a single issue for a project given project.</para>
    ///     <para>url like GET /projects/:id/issues/:issue_id</para>
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="issueIid"></param>
    /// <returns>The issue that corresponds to the project id and issue id</returns>
    Issue Get(long projectId, long issueIid);

    Task<Issue> GetAsync(long projectId, long issueIid, CancellationToken cancellationToken = default);

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
    IEnumerable<Issue> Get(long projectId, IssueQuery query);

    GitLabCollectionResponse<Issue> GetAsync(long projectId, IssueQuery query);

    /// <summary>
    ///     <para>Return a single issue, only for administrators.</para>
    ///     <para>url like GET /issues/:id</para>
    /// </summary>
    /// <param name="issueId"></param>
    /// <returns>The issue that corresponds to the issue id</returns>
    Issue GetById(long issueId);

    Task<Issue> GetByIdAsync(long issueId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add an issue with he proposed title to the GitLab list for the selected project id.
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
    /// <returns>A collection of the resource label events linked to this issue.</returns>
    IEnumerable<ResourceLabelEvent> ResourceLabelEvents(long projectId, long issueIid);

    GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long issueIid);

    /// <summary>
    /// Gets the resource milestone events.
    ///
    /// url like GET /projects/:id/issues/:issue_iid/resource_milestone_events
    ///
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>A collection of the resource milestone events linked to this issue.</returns>
    IEnumerable<ResourceMilestoneEvent> ResourceMilestoneEvents(long projectId, long issueIid);

    GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long issueIid);

    /// <summary>
    /// Gets the resource state events.
    ///
    /// url like GET /projects/:id/issues/:issue_iid/resource_state_events
    ///
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>A collection of the resource state events linked to this issue.</returns>
    IEnumerable<ResourceStateEvent> ResourceStateEvents(long projectId, long issueIid);

    GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(long projectId, long issueIid);

    /// <summary>
    /// Get all merge requests that are related to a particular issue.
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The list of MR that are related this issue.</returns>
    IEnumerable<MergeRequest> RelatedTo(long projectId, long issueIid);

    /// <summary>
    /// Get all Issues that are linked to a particular issue of particular project.
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueId">The id of the issue in the project's scope.</param>
    /// <returns>The list of Issues linked to this issue.</returns>
    GitLabCollectionResponse<Issue> LinkedToAsync(long projectId, long issueId);

    /// <summary>
    /// Create links between Issues.
    /// </summary>
    /// <param name="sourceProjectId">The project id.</param>
    /// <param name="sourceIssueId">The id of the issue in the project's scope.</param>
    /// <param name="targetProjectId">The target project id.</param>
    /// <param name="targetIssueId">The target id of the issue to link to.</param>
    bool CreateLinkBetweenIssues(long sourceProjectId, long sourceIssueId, long targetProjectId, long targetIssueId);

    GitLabCollectionResponse<MergeRequest> RelatedToAsync(long projectId, long issueIid);

    /// <summary>
    /// Get all merge requests that close a particular issue when merged.
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The list of MR that closed this issue.</returns>
    IEnumerable<MergeRequest> ClosedBy(long projectId, long issueIid);

    GitLabCollectionResponse<MergeRequest> ClosedByAsync(long projectId, long issueIid);

    /// <summary>
    /// Get time tracking statistics
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The time tracking statistics of the issue.</returns>
    Task<TimeStats> TimeStatsAsync(long projectId, long issueIid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Clone the issue to given project
    /// </summary>
    /// <param name="projectId">The project id</param>
    /// <param name="issueIid">The id of the issue in the project's scope</param>
    /// <param name="issueClone">Destination information</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Issue> CloneAsync(long projectId, long issueIid, IssueClone issueClone, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get Participants
    /// </summary>
    /// <param name="projectId">The project id</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The participants of the issue.</returns>
    IEnumerable<Participant> GetParticipants(ProjectId projectId, long issueIid);

    /// <summary>
    /// Subscribe to the issue.
    /// </summary>
    /// <param name="projectId">The project id</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The issue that corresponds to the project id and issue id</returns>
    Issue Subscribe(ProjectId projectId, long issueIid);

    /// <summary>
    /// Unsubscribe from the issue.
    /// </summary>
    /// <param name="projectId">The project id</param>
    /// <param name="issueIid">The id of the issue in the project's scope.</param>
    /// <returns>The issue that corresponds to the project id and issue id</returns>
    Issue Unsubscribe(ProjectId projectId, long issueIid);
}
