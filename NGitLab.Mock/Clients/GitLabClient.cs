using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
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

        public ICommitClient GetCommits(int projectId) => GetCommits((long)projectId);

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

        public IEventClient GetUserEvents(int userId) => new EventClient(Context, userId);

        public IEventClient GetProjectEvents(int projectId) => GetProjectEvents((long)projectId);

        public IEventClient GetProjectEvents(ProjectId projectId) => new EventClient(Context, null, projectId);

        public ICommitStatusClient GetCommitStatus(int projectId) => GetCommitStatus((long)projectId);

        public ICommitStatusClient GetCommitStatus(ProjectId projectId) => new CommitStatusClient(Context, projectId);

        public IEnvironmentClient GetEnvironmentClient(int projectId) => GetEnvironmentClient((long)projectId);

        public IEnvironmentClient GetEnvironmentClient(ProjectId projectId) => new EnvironmentClient(Context, projectId);

        public IClusterClient GetClusterClient(int projectId) => GetClusterClient((long)projectId);

        public IClusterClient GetClusterClient(ProjectId projectId) => new ClusterClient(Context, projectId);

        public IGroupBadgeClient GetGroupBadgeClient(int groupId) => GetGroupBadgeClient((long)groupId);

        public IGroupBadgeClient GetGroupBadgeClient(GroupId groupId) => new GroupBadgeClient(Context, groupId);

        public IGroupVariableClient GetGroupVariableClient(int groupId) => GetGroupVariableClient((long)groupId);

        public IGroupVariableClient GetGroupVariableClient(GroupId groupId) => new GroupVariableClient(Context, groupId);

        public IJobClient GetJobs(int projectId) => GetJobs((long)projectId);

        public IJobClient GetJobs(ProjectId projectId) => new JobClient(Context, projectId);

        public IMergeRequestClient GetMergeRequest(int projectId) => GetMergeRequest((long)projectId);

        public IMergeRequestClient GetMergeRequest(ProjectId projectId) => new MergeRequestClient(Context, projectId);

        public IMilestoneClient GetMilestone(int projectId) => GetMilestone((long)projectId);

        public IMilestoneClient GetMilestone(ProjectId projectId) => new MilestoneClient(Context, projectId, MilestoneScope.Projects);

        public IMilestoneClient GetGroupMilestone(int groupId) => GetGroupMilestone((long)groupId);

        public IMilestoneClient GetGroupMilestone(GroupId groupId) => new MilestoneClient(Context, groupId, MilestoneScope.Groups);

        public IReleaseClient GetReleases(int projectId) => GetReleases((long)projectId);

        public IReleaseClient GetReleases(ProjectId projectId) => new ReleaseClient(Context, projectId);

        public IPipelineClient GetPipelines(int projectId) => GetPipelines((long)projectId);

        public IPipelineClient GetPipelines(ProjectId projectId) => new PipelineClient(Context, jobClient: GetJobs(projectId), projectId: projectId);

        public IProjectBadgeClient GetProjectBadgeClient(int projectId) => GetProjectBadgeClient((long)projectId);

        public IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId) => new ProjectBadgeClient(Context, projectId);

        public IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId) => GetProjectIssueNoteClient((long)projectId);

        public IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId) => new ProjectIssueNoteClient(Context, projectId);

        public IProjectVariableClient GetProjectVariableClient(int projectId) => GetProjectVariableClient((long)projectId);

        public IProjectVariableClient GetProjectVariableClient(ProjectId projectId) => new ProjectVariableClient(Context, projectId);

        public IRepositoryClient GetRepository(int projectId) => GetRepository((long)projectId);

        public IRepositoryClient GetRepository(ProjectId projectId) => new RepositoryClient(Context, projectId);

        public ITriggerClient GetTriggers(int projectId) => GetTriggers((long)projectId);

        public ITriggerClient GetTriggers(ProjectId projectId) => new TriggerClient(Context, projectId);

        public IWikiClient GetWikiClient(int projectId) => GetWikiClient((long)projectId);

        public IWikiClient GetWikiClient(ProjectId projectId) => new WikiClient(Context, projectId);

        public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(int projectId) => GetProjectLevelApprovalRulesClient((long)projectId);

        public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId) => new ProjectLevelApprovalRulesClient(Context, projectId);

        public IProtectedBranchClient GetProtectedBranchClient(int projectId) => GetProtectedBranchClient((long)projectId);

        public IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId) => new ProtectedBranchClient(Context, projectId);

        public ISearchClient GetGroupSearchClient(int groupId) => GetGroupSearchClient((long)groupId);

        public ISearchClient GetGroupSearchClient(GroupId groupId) => new GroupSearchClient(Context, groupId);

        public ISearchClient GetProjectSearchClient(int projectId) => GetProjectSearchClient((long)projectId);

        public ISearchClient GetProjectSearchClient(ProjectId projectId) => new ProjectSearchClient(Context, projectId);
    }
}
