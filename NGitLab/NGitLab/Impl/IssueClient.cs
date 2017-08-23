using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class IssueClient : IIssueClient {
        const string IssuesUrl = "/issues";
        const string ProjectIssuesUrl = "/projects/{0}/issues";
        readonly API _api;

        public IssueClient(API api) {
            _api = api;
        }

        public IEnumerable<Issue> Owned() {
            return _api.Get().GetAll<Issue>(IssuesUrl);
        }

        public IEnumerable<Issue> ForProject(int projectId) {
            return _api.Get().GetAll<Issue>(string.Format(ProjectIssuesUrl, projectId));
        }
    }
}