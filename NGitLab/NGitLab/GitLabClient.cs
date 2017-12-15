using NGitLab.Impl;
using NGitLab.Models;
using System.Web;

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
        public static GitLabClient Connect(string hostUrl, string username, string password)
        {
            return Connect(hostUrl, username, password, Api.ApiVersion.V4_Oauth);
        }
        public static GitLabClient Connect(string hostUrl, string username, string password,Api.ApiVersion apiVersion)
        {
            var api = new Api(hostUrl, "");
            api._ApiVersion = apiVersion;
            string PrivateToken = null;
            switch (apiVersion)

            {
                case Api.ApiVersion.V3:
                case Api.ApiVersion.V4:
                    var session = api.Post().To<Session>($"/session?login={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}");
                    PrivateToken = session.PrivateToken;
                    break;
                case Api.ApiVersion.V3_Oauth:
                case Api.ApiVersion.V4_Oauth:
                    //https://docs.gitlab.com/ee/api/oauth2.html#resource-owner-password-credentials
                    var token    = api.Post().With(new oauth() { UserName =username, Password = password, GrantType = "password" }).To<token>(oauth.Url);
                    PrivateToken = token.AccessToken;
                    break;
                default:
                    break;
            }
            return Connect(hostUrl, PrivateToken, apiVersion);
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