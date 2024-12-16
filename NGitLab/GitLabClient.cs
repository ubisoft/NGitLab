using NGitLab.Extensions;
using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab;

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

    public ISearchClient AdvancedSearch { get; }

    public IGlobalJobClient Jobs { get; }

    public ILintClient Lint { get; }

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
        AdvancedSearch = new SearchClient(_api, "/search");
        Jobs = new GlobalJobsClient(_api);
        Lint = new LintClient(_api);
    }

    public IEventClient GetEvents()
        => new EventClient(_api, "events");

    public IEventClient GetUserEvents(long userId)
        => new EventClient(_api, $"users/{userId.ToStringInvariant()}/events");

    public IEventClient GetProjectEvents(ProjectId projectId)
        => new EventClient(_api, $"projects/{projectId.ValueAsUriParameter()}/events");

    public IRepositoryClient GetRepository(ProjectId projectId)
        => new RepositoryClient(_api, projectId);

    public ICommitClient GetCommits(ProjectId projectId)
        => new CommitClient(_api, projectId);

    public ICommitStatusClient GetCommitStatus(ProjectId projectId)
        => new CommitStatusClient(_api, projectId);

    public IPipelineClient GetPipelines(ProjectId projectId)
        => new PipelineClient(_api, projectId);

    public IPipelineScheduleClient GetPipelineSchedules(ProjectId projectId)
        => new PipelineScheduleClient(_api, projectId);

    public ITriggerClient GetTriggers(ProjectId projectId)
        => new TriggerClient(_api, projectId);

    public IJobClient GetJobs(ProjectId projectId)
        => new JobClient(_api, projectId);

    public IMergeRequestClient GetMergeRequest(ProjectId projectId)
        => new MergeRequestClient(_api, projectId);

    public IMergeRequestClient GetGroupMergeRequest(GroupId groupId)
        => new MergeRequestClient(_api, groupId);

    public IMilestoneClient GetMilestone(ProjectId projectId)
        => new MilestoneClient(_api, MilestoneScope.Projects, projectId);

    public IMilestoneClient GetGroupMilestone(GroupId groupId)
        => new MilestoneClient(_api, MilestoneScope.Groups, groupId);

    public IReleaseClient GetReleases(ProjectId projectId)
        => new ReleaseClient(_api, projectId);

    public IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId)
        => new ProjectIssueNoteClient(_api, projectId);

    public IEnvironmentClient GetEnvironmentClient(ProjectId projectId)
        => new EnvironmentClient(_api, projectId);

    public IClusterClient GetClusterClient(ProjectId projectId)
        => new ClusterClient(_api, projectId);

    public IWikiClient GetWikiClient(ProjectId projectId)
        => new WikiClient(_api, projectId);

    public IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId)
        => new ProjectBadgeClient(_api, projectId);

    public IGroupBadgeClient GetGroupBadgeClient(GroupId groupId)
        => new GroupBadgeClient(_api, groupId);

    public IProjectVariableClient GetProjectVariableClient(ProjectId projectId)
        => new ProjectVariableClient(_api, projectId);

    public IGroupVariableClient GetGroupVariableClient(GroupId groupId)
        => new GroupVariableClient(_api, groupId);

    public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId)
        => new ProjectLevelApprovalRulesClient(_api, projectId);

    public IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId)
        => new ProtectedBranchClient(_api, projectId);

    public IProtectedTagClient GetProtectedTagClient(ProjectId projectId)
        => new ProtectedTagClient(_api, projectId);

    public ISearchClient GetGroupSearchClient(GroupId groupId)
        => new SearchClient(_api, $"/groups/{groupId.ValueAsUriParameter()}/search");

    public ISearchClient GetProjectSearchClient(ProjectId projectId)
        => new SearchClient(_api, $"/projects/{projectId.ValueAsUriParameter()}/search");

    public IGroupHooksClient GetGroupHooksClient(GroupId groupId)
        => new GroupHooksClient(_api, groupId);
}
