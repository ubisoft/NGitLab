using System;
using NGitLab.Impl;

namespace NGitLab
{
    public class GitLabClient : IGitLabClient
    {
        private readonly API _api;

        public IUserClient Users { get; }
        public IProjectClient Projects { get; }
        public IIssueClient Issues { get; }
        public IGroupsClient Groups { get; }
        public INamespacesClient Namespaces { get; }
        public ILabelClient Labels { get; }
        public IRunnerClient Runners { get; }
        public IVersionClient Version { get; }

        public GitLabClient(string hostUrl, string apiToken)
            : this(new GitLabCredentials(hostUrl, apiToken))
        {
        }

        public GitLabClient(string hostUrl, string userName, string password)
            : this(new GitLabCredentials(hostUrl, userName, password))
        {
        }

        private GitLabClient(GitLabCredentials credentials)
        {
            _api = new API(credentials);
            Users = new UserClient(_api);
            Projects = new ProjectClient(_api);
            Runners = new RunnerClient(_api);
            Issues = new IssueClient(_api);
            Groups = new GroupsClient(_api);
            Namespaces = new NamespacesClient(_api);
            Labels = new LabelClient(_api);
            Version = new VersionClient(_api);
        }

        [Obsolete("Use gitlab client constructor instead")]
        public static GitLabClient Connect(string hostUrl, string apiToken)
        {
            return new GitLabClient(hostUrl, apiToken);
        }

        [Obsolete("Use gitlab client constructor instead")]
        public static GitLabClient Connect(string hostUrl, string username, string password)
        {
            return new GitLabClient(hostUrl, username, password);
        }

        public IRepositoryClient GetRepository(int projectId)
        {
            return new RepositoryClient(_api, projectId);
        }

        public ICommitClient GetCommits(int projectId)
        {
            return new CommitClient(_api, projectId);
        }
        
        public IPipelineClient GetPipelines(int projectId)
        {
            return new PipelineClient(_api, projectId);
        }

        public IMergeRequestClient GetMergeRequest(int projectId)
        {
            return new MergeRequestClient(_api, projectId);
        }

        public IMembersClient Members => new MembersClient(_api);
    }
}