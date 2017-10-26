using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab {
    public class GitLabClient {
        readonly Api api;

        public readonly INamespaceClient Groups;
        public readonly IIssueClient Issues;
        public readonly IProjectClient Projects;
        public readonly IUserClient Users;
        public string ApiToken => api.ApiToken;
        GitLabClient(string hostUrl, string apiToken) : this(hostUrl, apiToken, Api.ApiVersion.V4)
        {
        }
        GitLabClient(string hostUrl, string apiToken, Api.ApiVersion apiVersion)
        {
            api = new Api(hostUrl, apiToken);
            api._ApiVersion = apiVersion;
            Users = new UserClient(api);
            Projects = new ProjectClient(api);
            Issues = new IssueClient(api);
            Groups = new NamespaceClient(api);
        }
        public static GitLabClient Connect(string hostUrl, string username, string password,Api.ApiVersion apiVersion)
        {
            var api = new Api(hostUrl, "");
            api._ApiVersion = apiVersion;
            var session = api.Post().To<Session>($"/session?login={username}&password={password}");
            return Connect(hostUrl, session.PrivateToken,apiVersion);
        }
        public static GitLabClient Connect(string hostUrl, string username, string password)
        {
            var api = new Api(hostUrl, "");
            var session = api.Post().To<Session>($"/session?login={username}&password={password}");
            return Connect(hostUrl, session.PrivateToken);
        }
        public static GitLabClient Connect(string hostUrl, string apiToken,  Api.ApiVersion apiVersion)
        {
            return new GitLabClient(hostUrl, apiToken, apiVersion);
        }
        public static GitLabClient Connect(string hostUrl, string apiToken) {
            return new GitLabClient(hostUrl, apiToken);
        }

        public IRepositoryClient GetRepository(int projectId) {
            return new RepositoryClient(api, projectId);
        }

        public IMergeRequestClient GetMergeRequest(int projectId) {
            return new MergeRequestClient(api, projectId);
        }
    }
}