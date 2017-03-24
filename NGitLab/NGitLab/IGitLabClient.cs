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
        IBuildClient GetBuilds(int projectId);
        IPipelineClient GetPipelines(int projectId);
        IMergeRequestClient GetMergeRequest(int projectId);
        IMembersClient Members { get; }
    }
}