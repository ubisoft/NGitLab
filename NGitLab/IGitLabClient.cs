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
    IEventClient GetUserEvents(long userId);

    /// <summary>
    /// Returns the events that occurred in the specified project.
    /// </summary>
    IEventClient GetProjectEvents(ProjectId projectId);

    IRepositoryClient GetRepository(ProjectId projectId);

    ICommitClient GetCommits(ProjectId projectId);

    ICommitStatusClient GetCommitStatus(ProjectId projectId);

    IPipelineClient GetPipelines(ProjectId projectId);

    IPipelineScheduleClient GetPipelineSchedules(ProjectId projectId);

    ITriggerClient GetTriggers(ProjectId projectId);

    IJobClient GetJobs(ProjectId projectId);

    IMergeRequestClient GetMergeRequest(ProjectId projectId);

    IMergeRequestClient GetGroupMergeRequest(GroupId groupId);

    IMilestoneClient GetMilestone(ProjectId projectId);

    IMilestoneClient GetGroupMilestone(GroupId groupId);

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

    IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId);

    IEnvironmentClient GetEnvironmentClient(ProjectId projectId);

    IClusterClient GetClusterClient(ProjectId projectId);

    IWikiClient GetWikiClient(ProjectId projectId);

    IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId);

    IGroupBadgeClient GetGroupBadgeClient(GroupId groupId);

    IProjectVariableClient GetProjectVariableClient(ProjectId projectId);

    IGroupVariableClient GetGroupVariableClient(GroupId groupId);

    IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId);

    IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId);

    IProtectedTagClient GetProtectedTagClient(ProjectId projectId);

    public ISearchClient GetGroupSearchClient(GroupId groupId);

    public ISearchClient GetProjectSearchClient(ProjectId projectId);

    public IGroupHooksClient GetGroupHooksClient(GroupId groupId);
}
