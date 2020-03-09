namespace NGitLab
{
    public interface IGitLabClient
    {
        IUserClient Users { get; }

        IProjectClient Projects { get; }

        IIssueClient Issues { get; }

        IGroupsClient Groups { get; }

        ILabelClient Labels { get; }

        IRunnerClient Runners { get; }

        IRepositoryClient GetRepository(int projectId);

        ICommitClient GetCommits(int projectId);

        ICommitStatusClient GetCommitStatus(int projectId);

        IPipelineClient GetPipelines(int projectId);

        ITriggerClient GetTriggers(int projectId);

        IJobClient GetJobs(int projectId);

        IMergeRequestClient GetMergeRequest(int projectId);

        IMilestoneClient GetMilestone(int projectId);

        IMembersClient Members { get; }

        IVersionClient Version { get; }

        INamespacesClient Namespaces { get; }

        ISnippetClient Snippets { get; }

        ISystemHookClient SystemHooks { get; }

        IProjectIssueNoteClient GetProjectIssueNoteClient(int projectId);

        IEnvironmentClient GetEnvironmentClient(int projectId);

        IClusterClient GetClusterClient(int projectId);

        IWikiClient GetWikiClient(int projectId);

        IProjectBadgeClient GetProjectBadgeClient(int projectId);

        IGroupBadgeClient GetGroupBadgeClient(int groupId);

        IProjectVariableClient GetProjectVariableClient(int projectId);

        IGroupVariableClient GetGroupVariableClient(int groupId);
    }
}
