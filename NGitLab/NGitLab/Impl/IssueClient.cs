using System.Collections.Generic;
using NGitLab.Models;
using System.Linq;

namespace NGitLab.Impl
{
    public class IssueClient : IIssueClient
    {
        private const string IssuesUrl = "/issues";
        private const string ProjectIssuesUrl = "/projects/{0}/issues";
        private const string SingleIssueUrl = "/projects/{0}/issues/{1}";
        
        private readonly API _api;

        public IssueClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Issue> Owned
        {
            get
            {
                return _api.Get().GetAll<Issue>(IssuesUrl);
            }
        }

        public IEnumerable<Issue> ForProject(int projectId)
        {
            return _api.Get().GetAll<Issue>(string.Format(ProjectIssuesUrl, projectId));
        }

        public Issue Get(int projectId, int issueId)
        {
            return _api.Get().To<Issue>(string.Format(SingleIssueUrl, projectId, issueId));
        }

        public Issue Create(IssueCreate issueCreate)
        {
            return _api.Post().With(issueCreate).To<Issue>(string.Format(ProjectIssuesUrl, issueCreate.Id));
        }
        
        public Issue Edit(IssueEdit issueEdit)
        {
            return _api.Put().With(issueEdit).To<Issue>(string.Format(SingleIssueUrl, issueEdit.Id, issueEdit.IssueId));
        }

        
    }
}
