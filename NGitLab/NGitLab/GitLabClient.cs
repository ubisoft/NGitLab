using NGitLab.Impl;

namespace NGitLab
{
    public class GitLabClient
    {
        private GitLabClient(string hostUrl, string apiToken)
        {
            _api = new API(hostUrl, apiToken);
            Users = new UserClient(_api);
            Projects = new ProjectClient(_api);
        }

        public static GitLabClient Connect(string hostUrl, string apiToken)
        {
            return new GitLabClient(hostUrl, apiToken);
        }

        private readonly API _api;

        public readonly IUserClient Users;
        public readonly IProjectClient Projects;

        public IRepositoryClient GetRepository(int projectId)
        {
            return new RepositoryClient(_api, projectId);
        }
    }
}