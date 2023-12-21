using System.ComponentModel;
using NGitLab.Models;

namespace NGitLab;

public interface IGitLabClient
{
    IUserClient Users { get; }

    IProjectClient Projects { get; }

    IIssueClient Issues { get; }

    IGroupsClient Groups { get; }

    IGlobalJobClient Jobs { get; }

    ILabelClient Labels { get; }

    IRunnerClient Runners { get; }

    IMergeRequestClient MergeRequests { get; }

    ILintClient Lint { get; }

    /// <summary>
    /// All the user events of GitLab (can be scoped for the current user).
    /// </summary>
    IEventClient GetEvents();

    /// <summary>
    /// Returns the events done by the specified user.
    /// </summary>
    IEventClient GetUserEvents(int userId);

    /// <summary>
    /// Returns the events that occurred in the specified project.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    IEventClient GetProjectEvents(int projectId);

    /// <summary>
    /// Returns the events that occurred in the specified project.
    /// </summary>
    IEventClient GetProjectEvents(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IRepositoryClient GetRepository(int projectId);

    IRepositoryClient GetRepository(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    ICommitClient GetCommits(int projectId);

    ICommitClient GetCommits(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    ICommitStatusClient GetCommitStatus(int projectId);

    ICommitStatusClient GetCommitStatus(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IPipelineClient GetPipelines(int projectId);

    IPipelineClient GetPipelines(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    ITriggerClient GetTriggers(int projectId);

    ITriggerClient GetTriggers(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IJobClient GetJobs(int projectId);

    IJobClient GetJobs(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IMergeRequestClient GetMergeRequest(int projectId);

    IMergeRequestClient GetMergeRequest(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IMilestoneClient GetMilestone(int projectId);

    IMilestoneClient GetMilestone(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IMilestoneClient GetGroupMilestone(int groupId);

    IMilestoneClient GetGroupMilestone(GroupId groupId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IReleaseClient GetReleases(int projectId);

    IReleaseClient GetReleases(ProjectId projectId);

    IMembersClient Members { get; }

    IVersionClient Version { get; }

    INamespacesClient Namespaces { get; }

    ISnippetClient Snippets { get; }

    ISystemHookClient SystemHooks { get; }

    IDeploymentClient Deployments { get; }

    IEpicClient Epics { get; }

    IGraphQLClient GraphQL { get; }

    ISearchClient AdvancedSearch { get; }

    [EditorBrowsable(EditorBrowsableState.Never)]
    IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId);

    IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IEnvironmentClient GetEnvironmentClient(int projectId);

    IEnvironmentClient GetEnvironmentClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IClusterClient GetClusterClient(int projectId);

    IClusterClient GetClusterClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IWikiClient GetWikiClient(int projectId);

    IWikiClient GetWikiClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IProjectBadgeClient GetProjectBadgeClient(int projectId);

    IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IGroupBadgeClient GetGroupBadgeClient(int groupId);

    IGroupBadgeClient GetGroupBadgeClient(GroupId groupId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IProjectVariableClient GetProjectVariableClient(int projectId);

    IProjectVariableClient GetProjectVariableClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IGroupVariableClient GetGroupVariableClient(int groupId);

    IGroupVariableClient GetGroupVariableClient(GroupId groupId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(int projectId);

    IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    IProtectedBranchClient GetProtectedBranchClient(int projectId);

    IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId);

    IProtectedTagClient GetProtectedTagClient(ProjectId projectId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ISearchClient GetGroupSearchClient(int groupId);

    public ISearchClient GetGroupSearchClient(GroupId groupId);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ISearchClient GetProjectSearchClient(int projectId);

    public ISearchClient GetProjectSearchClient(ProjectId projectId);
}
