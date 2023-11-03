using System;
using NGitLab.Models;

namespace NGitLab
{
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
        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IEventClient GetProjectEvents(int projectId);

        /// <summary>
        /// Returns the events that occurred in the specified project.
        /// </summary>
        IEventClient GetProjectEvents(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IRepositoryClient GetRepository(int projectId);

        IRepositoryClient GetRepository(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        ICommitClient GetCommits(int projectId);

        ICommitClient GetCommits(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        ICommitStatusClient GetCommitStatus(int projectId);

        ICommitStatusClient GetCommitStatus(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IPipelineClient GetPipelines(int projectId);

        IPipelineClient GetPipelines(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        ITriggerClient GetTriggers(int projectId);

        ITriggerClient GetTriggers(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IJobClient GetJobs(int projectId);

        IJobClient GetJobs(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IMergeRequestClient GetMergeRequest(int projectId);

        IMergeRequestClient GetMergeRequest(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IMilestoneClient GetMilestone(int projectId);

        IMilestoneClient GetMilestone(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as groupId instead.")]
        IMilestoneClient GetGroupMilestone(int groupId);

        IMilestoneClient GetGroupMilestone(GroupId groupId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
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

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId);

        IProjectIssueNoteClient GetProjectIssueNoteClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IEnvironmentClient GetEnvironmentClient(int projectId);

        IEnvironmentClient GetEnvironmentClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IClusterClient GetClusterClient(int projectId);

        IClusterClient GetClusterClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IWikiClient GetWikiClient(int projectId);

        IWikiClient GetWikiClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IProjectBadgeClient GetProjectBadgeClient(int projectId);

        IProjectBadgeClient GetProjectBadgeClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as groupId instead.")]
        IGroupBadgeClient GetGroupBadgeClient(int groupId);

        IGroupBadgeClient GetGroupBadgeClient(GroupId groupId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IProjectVariableClient GetProjectVariableClient(int projectId);

        IProjectVariableClient GetProjectVariableClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as groupId instead.")]
        IGroupVariableClient GetGroupVariableClient(int groupId);

        IGroupVariableClient GetGroupVariableClient(GroupId groupId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(int projectId);

        IProjectLevelApprovalRulesClient GetProjectLevelApprovalRulesClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        IProtectedBranchClient GetProtectedBranchClient(int projectId);

        IProtectedBranchClient GetProtectedBranchClient(ProjectId projectId);

        [Obsolete("Use long or namespaced path string as groupId instead.")]
        public ISearchClient GetGroupSearchClient(int groupId);

        public ISearchClient GetGroupSearchClient(GroupId groupId);

        [Obsolete("Use long or namespaced path string as projectId instead.")]
        public ISearchClient GetProjectSearchClient(int projectId);

        public ISearchClient GetProjectSearchClient(ProjectId projectId);
    }
}
