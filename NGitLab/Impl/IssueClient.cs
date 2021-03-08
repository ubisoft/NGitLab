using System.Collections.Generic;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class IssueClient : IIssueClient
    {
        private const string IssuesUrl = "/issues";
        private const string ProjectIssuesUrl = "/projects/{0}/issues";
        private const string SingleIssueUrl = "/projects/{0}/issues/{1}";
        private const string ResourceLabelEventUrl = "/projects/{0}/issues/{1}/resource_label_events";
        private const string RelatedToUrl = "/projects/{0}/issues/{1}/related_merge_requests";
        private const string ClosedByUrl = "/projects/{0}/issues/{1}/closed_by";

        private readonly API _api;

        public IssueClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Issue> Owned => _api.Get().GetAll<Issue>(IssuesUrl);

        public IEnumerable<Issue> ForProject(int projectId)
        {
            return _api.Get().GetAll<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId));
        }

        public Issue Get(int projectId, int issueId)
        {
            return _api.Get().To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, projectId, issueId));
        }

        public IEnumerable<Issue> Get(int projectId, IssueQuery query)
        {
            return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
        }

        public IEnumerable<Issue> Get(IssueQuery query)
        {
            return Get(IssuesUrl, query);
        }

        private IEnumerable<Issue> Get(string url, IssueQuery query)
        {
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
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
            url = Utils.AddParameter(url, "search", query.Search);

            return _api.Get().GetAll<Issue>(url);
        }

        public Issue Create(IssueCreate issueCreate)
        {
            return _api.Post().With(issueCreate).To<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, issueCreate.Id));
        }

        public Issue Edit(IssueEdit issueEdit)
        {
            return _api.Put().With(issueEdit).To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, issueEdit.Id, issueEdit.IssueId));
        }

        public IEnumerable<ResourceLabelEvent> ResourceLabelEvents(int projectId, int issueIid)
        {
            return _api.Get().GetAll<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, issueIid));
        }

        public IEnumerable<MergeRequest> RelatedTo(int projectId, int issueIid)
        {
            return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, RelatedToUrl, projectId, issueIid));
        }

        public IEnumerable<MergeRequest> ClosedBy(int projectId, int issueIid)
        {
            return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, ClosedByUrl, projectId, issueIid));
        }
    }
}
