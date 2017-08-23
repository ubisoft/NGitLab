using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class IssueClient : IIssueClient {
        const string IssuesUrl = "/issues";
        const string ProjectIssuesUrl = "/projects/{0}/issues";
        readonly Api api;

        public IssueClient(Api api) {
            this.api = api;
        }

        public IEnumerable<Issue> Owned() {
            return api.Get().GetAll<Issue>(IssuesUrl);
        }

        public IEnumerable<Issue> ForProject(int projectId) {
            return api.Get().GetAll<Issue>(string.Format(ProjectIssuesUrl, projectId));
        }
    }
}