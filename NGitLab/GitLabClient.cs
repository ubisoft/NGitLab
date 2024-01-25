using System;
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

    [Obsolete("Use GitLabClient constructor instead")]
    public static GitLabClient Connect(string hostUrl, string apiToken)
        => new(hostUrl, apiToken);

    [Obsolete("Use GitLabClient constructor instead")]
    public static GitLabClient Connect(string hostUrl, string username, string password)
        => new(hostUrl, username, password);

    public IEventClient GetEvents()
        => new EventClient(_api, "events");

    public IEventClient GetUserEvents(int userId)
        => new EventClient(_api, $"users/{userId.ToStringInvariant()}/events");

    public IEventClient GetProjectEvents(int projectId)
        => GetProjectEvents((long)projectId);

    public IEventClient GetProjectEvents(ProjectId projectId)
        => new EventClient(_api, $"projects/{projectId.ValueAsUriParameter()}/events");

    public IRepositoryClient GetRepository(int projectId)
        => GetRepository((long)projectId);

    public IRepositoryClient GetRepository(ProjectId projectId)
        => new RepositoryClient(_api, projectId);

    public ICommitClient GetCommits(int projectId)
        => GetCommits((long)projectId);

    public ICommitClient GetCommits(ProjectId projectId)
        => new CommitClient(_api, projectId);

    public ICommitStatusClient GetCommitStatus(int projectId)
        => GetCommitStatus((long)projectId);

    public ICommitStatusClient GetCommitStatus(ProjectId projectId)
        => new CommitStatusClient(_api, projectId);

    public IPipelineClient GetPipelines(int projectId)
        => GetPipelines((long)projectId);

    public IPipelineClient GetPipelines(ProjectId projectId)
        => new PipelineClient(_api, projectId);

    public ITriggerClient GetTriggers(int projectId)
        => GetTriggers((long)projectId);

    public ITriggerClient GetTriggers(ProjectId projectId)
        => new TriggerClient(_api, projectId);

    public IJobClient GetJobs(int projectId)
        => GetJobs((long)projectId);

    public IJobClient GetJobs(ProjectId projectId)
        => new JobClient(_api, projectId);

    public IMergeRequestClient GetMergeRequest(int projectId)
        => GetMergeRequest((long)projectId);

    public IMergeRequestClient GetMergeRequest(ProjectId projectId)
        => new MergeRequestClient(_api, projectId);

    public IMergeRequestClient GetGroupMergeRequest(int groupId)
        => GetGroupMergeRequest((long)groupId);

    public IMergeRequestClient GetGroupMergeRequest(GroupId groupId)
        => new MergeRequestClient(groupId, _api);

    public IMilestoneClient GetMilestone(int projectId)
        => GetMilestone((long)projectId);

    public IMilestoneClient GetMilestone(ProjectId projectId)
        => new MilestoneClient(_api, MilestoneScope.Projects, projectId);

    public IMilestoneClient GetGroupMilestone(int groupId)
        => GetGroupMilestone((long)groupId);

    public IMilestoneClient GetGroupMilestone(GroupId groupId)
        => new MilestoneClient(_api, MilestoneScope.Groups, groupId);

    public IReleaseClient GetReleases(int projectId)
        => GetReleases((long)projectId);

    public IReleaseClient GetReleases(ProjectId projectId)
        => new ReleaseClient(_api, projectId);

    public IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId)
        => GetProjectIssueNoteClient((long)projectId);

    public IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId)
        => new ProjectIssueNoteClient(_api, projectId);

    public IEnvironmentClient GetEnvironmentClient(int projectId)
        => GetEnvironmentClient((long)projectId);

    public IEnvironmentClient GetEnvironmentClient(ProjectId projectId)
        => new EnvironmentClient(_api, projectId);

    public IClusterClient GetClusterClient(int projectId)
        => GetClusterClient((long)projectId);

    public IClusterClient GetClusterClient(ProjectId projectId)
        => new ClusterClient(_api, projectId);

    public IWikiClient GetWikiClient(int projectId)
        => GetWikiClient((long)projectId);

    public IWikiClient GetWikiClient(ProjectId projectId)
        => new WikiClient(_api, projectId);

    public IProjectBadgeClient GetProjectBadgeClient(int projectId)
        => GetProjectBadgeClient((long)projectId);

    public IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId)
        => new ProjectBadgeClient(_api, projectId);

    public IGroupBadgeClient GetGroupBadgeClient(int groupId)
        => GetGroupBadgeClient((long)groupId);

    public IGroupBadgeClient GetGroupBadgeClient(GroupId groupId)
        => new GroupBadgeClient(_api, groupId);

    public IProjectVariableClient GetProjectVariableClient(int projectId)
        => GetProjectVariableClient((long)projectId);

    public IProjectVariableClient GetProjectVariableClient(ProjectId projectId)
        => new ProjectVariableClient(_api, projectId);

    public IGroupVariableClient GetGroupVariableClient(int groupId)
        => GetGroupVariableClient((long)groupId);

    public IGroupVariableClient GetGroupVariableClient(GroupId groupId)
        => new GroupVariableClient(_api, groupId);

    public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(int projectId)
        => GetProjectLevelApprovalRulesClient((long)projectId);

    public IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId)
        => new ProjectLevelApprovalRulesClient(_api, projectId);

    public IProtectedBranchClient GetProtectedBranchClient(int projectId)
        => GetProtectedBranchClient((long)projectId);

    public IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId)
        => new ProtectedBranchClient(_api, projectId);

    public IProtectedTagClient GetProtectedTagClient(ProjectId projectId)
        => new ProtectedTagClient(_api, projectId);

    public ISearchClient GetGroupSearchClient(int groupId)
        => GetGroupSearchClient((long)groupId);

    public ISearchClient GetGroupSearchClient(GroupId groupId)
        => new SearchClient(_api, $"/groups/{groupId.ValueAsUriParameter()}/search");

    public ISearchClient GetProjectSearchClient(int projectId)
        => GetProjectSearchClient((long)projectId);

    public ISearchClient GetProjectSearchClient(ProjectId projectId)
        => new SearchClient(_api, $"/projects/{projectId.ValueAsUriParameter()}/search");
}
