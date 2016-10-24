using System.Security.Policy;
using NGitLab.Impl;
using NGitLab.Models;

namespace NGitLab
{
    public class GitLabClient
    {
        private readonly API _api;

        public readonly IUserClient Users;
        public readonly IProjectClient Projects;
        public readonly IIssueClient Issues;
        public readonly INamespaceClient Groups;
        public readonly ILabelClient Labels;
        public readonly IRunnerClient Runners;

        public static IHttpRequestor HttpRequestor { get; set; }
        
        private GitLabClient(IHttpRequestor httpRequestor)
        {
            _api = new API(httpRequestor);
            Users = new UserClient(_api);
            Projects = new ProjectClient(_api);
            Runners = new RunnerClient(_api);
            Issues = new IssueClient(_api);
            Groups = new NamespaceClient(_api);
            Labels = new LabelClient(_api);
        }

        public static GitLabClient Connect(string hostUrl, string apiToken)
        {
            if(HttpRequestor == null)
            {
                HttpRequestor = new HttpRequestor(hostUrl, apiToken);
            }

            return new GitLabClient(HttpRequestor);
        }

        public static GitLabClient Connect(string hostUrl, string username, string password)
        {
            var api = new API(new HttpRequestor(hostUrl, null));
            var session = api.Post().To<Session>($"/session?login={System.Web.HttpUtility.UrlEncode(username)}&password={System.Web.HttpUtility.UrlEncode(password)}");

            if (HttpRequestor == null)
            {
                HttpRequestor = new HttpRequestor(hostUrl, session.PrivateToken);
            }

            return new GitLabClient(HttpRequestor);
        }

        public IRepositoryClient GetRepository(int projectId)
        {
            return new RepositoryClient(_api, projectId);
        }

        public IMergeRequestClient GetMergeRequest(int projectId)
        {
            return new MergeRequestClient(_api, projectId);
        }
    }
}