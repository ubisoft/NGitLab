using System.Collections.Generic;
using NGitLab.Models;

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

        public IEnumerable<Issue> Owned => _api.Get().GetAll<Issue>(IssuesUrl);

        public IEnumerable<Issue> ForProject(int projectId)
        {
            return _api.Get().GetAll<Issue>(string.Format(ProjectIssuesUrl, projectId));
        }

        public Issue Get(int projectId, int issueId)
        {
            return _api.Get().To<Issue>(string.Format(SingleIssueUrl, projectId, issueId));
        }

        public IEnumerable<Issue> Get(IssueQuery query)
        {
            var url = IssuesUrl;

            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "milestone", query.Milestone);
            url = Utils.AddParameter(url, "labels", query.Labels);
            url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
            url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
            url = Utils.AddParameter(url, "updated_after", query.UpdatedAfter);
            url = Utils.AddParameter(url, "updated_before", query.UpdatedBefore);
            url = Utils.AddParameter(url, "scope", query.Scope);
            url = Utils.AddParameter(url, "author_id", query.AuthorId);
            if (query.AssigneeId == 0) // unassigned. In the next version of GitLab, 0 or empty mean unassigned, but in the current version we must use an empty value.
            {
                url = Utils.AddParameter(url, "assignee_id", "");
            }
            else
            {
                url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
            }
            url = Utils.AddParameter(url, "search", query.Search);

            return _api.Get().GetAll<Issue>(url);
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
