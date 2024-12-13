using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class GitLabClient : ClientBase, IGitLabClient
{
    private IGraphQLClient _graphQLClient;

    public GitLabClient(ClientContext context)
        : base(context)
    {
    }

    public IGroupsClient Groups => new GroupClient(Context);

    public IUserClient Users => new UserClient(Context);

    public IProjectClient Projects => new ProjectClient(Context);

    public IMembersClient Members => new MembersClient(Context);

    public ICommitClient GetCommits(ProjectId projectId) => new CommitClient(Context, projectId);

    public IIssueClient Issues => new IssueClient(Context);

    public ILabelClient Labels => new LabelClient(Context);

    public IRunnerClient Runners => new RunnerClient(Context);

    public IVersionClient Version => new VersionClient(Context);

    public INamespacesClient Namespaces => new NamespacesClient(Context);

    public ISnippetClient Snippets => new SnippetClient(Context);

    public ISystemHookClient SystemHooks => new SystemHookClient(Context);

    public IDeploymentClient Deployments { get; }

    public IEpicClient Epics { get; }

    public IMergeRequestClient MergeRequests => new MergeRequestClient(Context);

    public IGlobalJobClient Jobs => new GlobalJobsClient(Context);

    public IGraphQLClient GraphQL
    {
        get => _graphQLClient ??= Server.DefaultGraphQLClient ??= new GraphQLClient(Context);
        internal set => _graphQLClient = value;
    }

    public ISearchClient AdvancedSearch => new AdvancedSearchClient(Context);

    public ILintClient Lint => new LintClient(Context);

    public IEventClient GetEvents() => new EventClient(Context);

    public IEventClient GetUserEvents(long userId) => new EventClient(Context, userId);

    public IEventClient GetProjectEvents(ProjectId projectId) => new EventClient(Context, null, projectId);

    public ICommitStatusClient GetCommitStatus(ProjectId projectId) => new CommitStatusClient(Context, projectId);

    public IEnvironmentClient GetEnvironmentClient(ProjectId projectId) => new EnvironmentClient(Context, projectId);

    public IClusterClient GetClusterClient(ProjectId projectId) => new ClusterClient(Context, projectId);

    public IGroupBadgeClient GetGroupBadgeClient(GroupId groupId) => new GroupBadgeClient(Context, groupId);

    public IGroupVariableClient GetGroupVariableClient(GroupId groupId) => new GroupVariableClient(Context, groupId);

    public IJobClient GetJobs(ProjectId projectId) => new JobClient(Context, projectId);

    public IMergeRequestClient GetMergeRequest(ProjectId projectId) => new MergeRequestClient(Context, projectId);

    public IMergeRequestClient GetGroupMergeRequest(GroupId groupId) => new MergeRequestClient(Context, groupId);

    public IMilestoneClient GetMilestone(ProjectId projectId) => new MilestoneClient(Context, projectId, MilestoneScope.Projects);

    public IMilestoneClient GetGroupMilestone(GroupId groupId) => new MilestoneClient(Context, groupId, MilestoneScope.Groups);

    public IReleaseClient GetReleases(ProjectId projectId) => new ReleaseClient(Context, projectId);

    public IPipelineClient GetPipelines(ProjectId projectId) => new PipelineClient(Context, jobClient: GetJobs(projectId), projectId: projectId);

    public IPipelineScheduleClient GetPipelineSchedules(ProjectId projectId)
        => new PipelineScheduleClient(Context, projectId);

    public IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId) => new ProjectBadgeClient(Context, projectId);

    public IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId) => new ProjectIssueNoteClient(Context, projectId);

    public IProjectVariableClient GetProjectVariableClient(ProjectId projectId) => new ProjectVariableClient(Context, projectId);

    public IRepositoryClient GetRepository(ProjectId projectId) => new RepositoryClient(Context, projectId);

    public ITriggerClient GetTriggers(ProjectId projectId) => new TriggerClient(Context, projectId);

    public IWikiClient GetWikiClient(ProjectId projectId) => new WikiClient(Context, projectId);

    public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId) => new ProjectLevelApprovalRulesClient(Context, projectId);

    public IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId) => new ProtectedBranchClient(Context, projectId);

    public IProtectedTagClient GetProtectedTagClient(ProjectId projectId) => new ProtectedTagClient(Context, projectId);

    public ISearchClient GetGroupSearchClient(GroupId groupId) => new GroupSearchClient(Context, groupId);

    public ISearchClient GetProjectSearchClient(ProjectId projectId) => new ProjectSearchClient(Context, projectId);

    public IGroupHooksClient GetGroupHooksClient(GroupId groupId) => new GroupHooksClient(Context, groupId);
}
