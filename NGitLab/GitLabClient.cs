using System;
using NGitLab.Extensions;
using NGitLab.Impl;

namespace NGitLab
{
    public class GitLabClient : IGitLabClient
    {
        private readonly API _api;

        public IUserClient Users { get; }

        public IProjectClient Projects { get; }

        public IMergeRequestClient MergeRequests { get; }

        public IIssueClient Issues { get; }

        public IGroupsClient Groups { get; }

        public INamespacesClient Namespaces { get; }

        public ILabelClient Labels { get; }

        public IRunnerClient Runners { get; }

        public IVersionClient Version { get; }

        public ISnippetClient Snippets { get; }

        public IMembersClient Members { get; }

        public ISystemHookClient SystemHooks { get; }

        public IDeploymentClient Deployments { get; }

        public IEpicClient Epics { get; }

        public IGraphQLClient GraphQL { get; }

        public RequestOptions Options
        {
            get => _api.RequestOptions;
            set => _api.RequestOptions = value;
        }

        public GitLabClient(string hostUrl, string apiToken)
            : this(hostUrl, apiToken, RequestOptions.Default)
        {
        }

        public GitLabClient(string hostUrl, string userName, string password)
            : this(hostUrl, userName, password, RequestOptions.Default)
        {
        }

        public GitLabClient(string hostUrl, string apiToken, RequestOptions options)
            : this(new GitLabCredentials(hostUrl, apiToken), options)
        {
        }

        public GitLabClient(string hostUrl, string userName, string password, RequestOptions options)
            : this(new GitLabCredentials(hostUrl, userName, password), options)
        {
        }

        private GitLabClient(GitLabCredentials credentials, RequestOptions options)
        {
            _api = new API(credentials, options);
            Users = new UserClient(_api);
            Projects = new ProjectClient(_api);
            MergeRequests = new MergeRequestClient(_api);
            Runners = new RunnerClient(_api);
            Issues = new IssueClient(_api);
            Groups = new GroupsClient(_api);
            Namespaces = new NamespacesClient(_api);
            Labels = new LabelClient(_api);
            Version = new VersionClient(_api);
            Snippets = new SnippetClient(_api);
            Members = new MembersClient(_api);
            SystemHooks = new SystemHookClient(_api);
            Deployments = new DeploymentClient(_api);
            Epics = new EpicClient(_api);
            GraphQL = new GraphQLClient(_api);
        }

        [Obsolete("Use GitLabClient constructor instead")]
        public static GitLabClient Connect(string hostUrl, string apiToken)
        {
            return new GitLabClient(hostUrl, apiToken);
        }

        [Obsolete("Use GitLabClient constructor instead")]
        public static GitLabClient Connect(string hostUrl, string username, string password)
        {
            return new GitLabClient(hostUrl, username, password);
        }

        public IEventClient GetEvents()
        {
            return new EventClient(_api, "events");
        }

        public IEventClient GetUserEvents(int userId)
        {
            return new EventClient(_api, $"users/{userId.ToStringInvariant()}/events");
        }

        public IEventClient GetProjectEvents(int projectId)
        {
            return new EventClient(_api, $"projects/{projectId.ToStringInvariant()}/events");
        }

        public IRepositoryClient GetRepository(int projectId)
        {
            return new RepositoryClient(_api, projectId);
        }

        public ICommitClient GetCommits(int projectId)
        {
            return new CommitClient(_api, projectId);
        }

        public ICommitStatusClient GetCommitStatus(int projectId)
        {
            return new CommitStatusClient(_api, projectId);
        }

        public IPipelineClient GetPipelines(int projectId)
        {
            return new PipelineClient(_api, projectId);
        }

        public ITriggerClient GetTriggers(int projectId)
        {
            return new TriggerClient(_api, projectId);
        }

        public IJobClient GetJobs(int projectId)
        {
            return new JobClient(_api, projectId);
        }

        public IMergeRequestClient GetMergeRequest(int projectId)
        {
            return new MergeRequestClient(_api, projectId);
        }

        public IMilestoneClient GetMilestone(int projectId)
        {
            return new MilestoneClient(_api, projectId);
        }

        public IReleaseClient GetReleases(int projectId)
        {
            return new ReleaseClient(_api, projectId);
        }

        public IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId)
        {
            return new ProjectIssueNoteClient(_api, projectId);
        }

        public IEnvironmentClient GetEnvironmentClient(int projectId)
        {
            return new EnvironmentClient(_api, projectId);
        }

        public IClusterClient GetClusterClient(int projectId)
        {
            return new ClusterClient(_api, projectId);
        }

        public IWikiClient GetWikiClient(int projectId)
        {
            return new WikiClient(_api, projectId);
        }

        public IProjectBadgeClient GetProjectBadgeClient(int projectId)
        {
            return new ProjectBadgeClient(_api, projectId);
        }

        public IGroupBadgeClient GetGroupBadgeClient(int groupId)
        {
            return new GroupBadgeClient(_api, groupId);
        }

        public IProjectVariableClient GetProjectVariableClient(int projectId)
        {
            return new ProjectVariableClient(_api, projectId);
        }

        public IGroupVariableClient GetGroupVariableClient(int groupId)
        {
            return new GroupVariableClient(_api, groupId);
        }

        public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(int projectId)
        {
            return new ProjectLevelApprovalRulesClient(_api, projectId);
        }

        public IProtectedBranchClient GetProtectedBranchClient(int projectId)
            => new ProtectedBranchClient(_api, projectId);
    }
}
