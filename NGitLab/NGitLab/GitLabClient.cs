using NGitLab.Impl;

namespace NGitLab
{
    public class GitLabClient
    {
        private GitLabClient(string hostUrl, string apiToken)
        {
            var api = new API(hostUrl, apiToken);
            Users = new UserClient(api);
            Projects = new ProjectClient(api);
        }

        public static GitLabClient Connect(string hostUrl, string apiToken)
        {
            return new GitLabClient(hostUrl, apiToken);
        }

        public readonly IUserClient Users;
        public readonly IProjectClient Projects;
    }
}