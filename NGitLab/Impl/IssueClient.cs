using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class IssueClient : IIssueClient
    {
        private const string IssuesUrl = "/issues";
        private const string IssueByIdUrl = "/issues/{0}";
        private const string LinkedIssuesByIdUrl = "/projects/{0}/issues/{1}/links";
        private const string CreateLinkBetweenIssuesUrl = "/projects/{0}/issues/{1}/links?target_project_id={2}&target_issue_iid={3}";
        private const string GroupIssuesUrl = "/groups/{0}/issues";
        private const string ProjectIssuesUrl = "/projects/{0}/issues";
        private const string SingleIssueUrl = "/projects/{0}/issues/{1}";
        private const string ResourceLabelEventUrl = "/projects/{0}/issues/{1}/resource_label_events";
        private const string ResourceMilestoneEventUrl = "/projects/{0}/issues/{1}/resource_milestone_events";
        private const string ResourceStateEventUrl = "/projects/{0}/issues/{1}/resource_state_events";
        private const string RelatedToUrl = "/projects/{0}/issues/{1}/related_merge_requests";
        private const string ClosedByUrl = "/projects/{0}/issues/{1}/closed_by";
        private const string TimeStatsUrl = "/projects/{0}/issues/{1}/time_stats";
        private const string CloneIssueUrl = "/projects/{0}/issues/{1}/clone";

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

        public GitLabCollectionResponse<Issue> ForProjectAsync(int projectId)
        {
            return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId));
        }

        public GitLabCollectionResponse<Issue> ForGroupsAsync(int groupId)
        {
            return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, GroupIssuesUrl, groupId));
        }

        public GitLabCollectionResponse<Issue> ForGroupsAsync(int groupId, IssueQuery query)
        {
            return Get(string.Format(CultureInfo.InvariantCulture, GroupIssuesUrl, groupId), query);
        }

        public Issue Get(int projectId, int issueIid)
        {
            return _api.Get().To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, projectId, issueIid));
        }

        public Task<Issue> GetAsync(int projectId, int issueIid, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, projectId, issueIid), cancellationToken);
        }

        public IEnumerable<Issue> Get(int projectId, IssueQuery query)
        {
            return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
        }

        public GitLabCollectionResponse<Issue> GetAsync(int projectId, IssueQuery query)
        {
            return Get(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, projectId), query);
        }

        public IEnumerable<Issue> Get(IssueQuery query)
        {
            return Get(IssuesUrl, query);
        }

        public GitLabCollectionResponse<Issue> GetAsync(IssueQuery query)
        {
            return Get(IssuesUrl, query);
        }

        public Issue GetById(int issueId)
        {
            return _api.Get().To<Issue>(string.Format(CultureInfo.InvariantCulture, IssueByIdUrl, issueId));
        }

        public Task<Issue> GetByIdAsync(int issueId, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, IssueByIdUrl, issueId), cancellationToken);
        }

        public Issue Create(IssueCreate issueCreate)
        {
            return _api.Post().With(issueCreate).To<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, issueCreate.ProjectId));
        }

        public Task<Issue> CreateAsync(IssueCreate issueCreate, CancellationToken cancellationToken = default)
        {
            return _api.Post().With(issueCreate).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, ProjectIssuesUrl, issueCreate.ProjectId), cancellationToken);
        }

        public Issue Edit(IssueEdit issueEdit)
        {
            return _api.Put().With(issueEdit).To<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, issueEdit.ProjectId, issueEdit.IssueId));
        }

        public Task<Issue> EditAsync(IssueEdit issueEdit, CancellationToken cancellationToken = default)
        {
            return _api.Put().With(issueEdit).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, SingleIssueUrl, issueEdit.ProjectId, issueEdit.IssueId), cancellationToken);
        }

        public IEnumerable<ResourceLabelEvent> ResourceLabelEvents(int projectId, int issueIid)
        {
            return _api.Get().GetAll<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, issueIid));
        }

        public GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(int projectId, int issueIid)
        {
            return _api.Get().GetAllAsync<ResourceLabelEvent>(string.Format(CultureInfo.InvariantCulture, ResourceLabelEventUrl, projectId, issueIid));
        }

        public IEnumerable<ResourceMilestoneEvent> ResourceMilestoneEvents(int projectId, int issueIid)
        {
            return _api.Get().GetAll<ResourceMilestoneEvent>(string.Format(CultureInfo.InvariantCulture, ResourceMilestoneEventUrl, projectId, issueIid));
        }

        public GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(int projectId, int issueIid)
        {
            return _api.Get().GetAllAsync<ResourceMilestoneEvent>(string.Format(CultureInfo.InvariantCulture, ResourceMilestoneEventUrl, projectId, issueIid));
        }

        public IEnumerable<ResourceStateEvent> ResourceStateEvents(int projectId, int issueIid)
        {
            return _api.Get().GetAll<ResourceStateEvent>(string.Format(CultureInfo.InvariantCulture, ResourceStateEventUrl, projectId, issueIid));
        }

        public GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(int projectId, int issueIid)
        {
            return _api.Get().GetAllAsync<ResourceStateEvent>(string.Format(CultureInfo.InvariantCulture, ResourceStateEventUrl, projectId, issueIid));
        }

        public IEnumerable<MergeRequest> RelatedTo(int projectId, int issueIid)
        {
            return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, RelatedToUrl, projectId, issueIid));
        }

        public GitLabCollectionResponse<Issue> LinkedTo(int projectId, int issueId)
        {
            return _api.Get().GetAllAsync<Issue>(string.Format(CultureInfo.InvariantCulture, LinkedIssuesByIdUrl, projectId, issueId));
        }

        public bool CreateLinkBetweenIssues(int sourceProjectId, int sourceIssueId, int targetProjectId,
            int targetIssueId)
        {
            _api.Post().Execute(string.Format(CultureInfo.InvariantCulture, CreateLinkBetweenIssuesUrl, sourceProjectId, sourceIssueId, targetProjectId, targetIssueId));
            return true;
        }

        public GitLabCollectionResponse<MergeRequest> RelatedToAsync(int projectId, int issueIid)
        {
            return _api.Get().GetAllAsync<MergeRequest>(string.Format(CultureInfo.InvariantCulture, RelatedToUrl, projectId, issueIid));
        }

        public IEnumerable<MergeRequest> ClosedBy(int projectId, int issueIid)
        {
            return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, ClosedByUrl, projectId, issueIid));
        }

        public GitLabCollectionResponse<MergeRequest> ClosedByAsync(int projectId, int issueIid)
        {
            return _api.Get().GetAllAsync<MergeRequest>(string.Format(CultureInfo.InvariantCulture, ClosedByUrl, projectId, issueIid));
        }

        public Task<TimeStats> TimeStatsAsync(int projectId, int issueIid, CancellationToken cancellationToken = default)
        {
            return _api.Get().ToAsync<TimeStats>(string.Format(CultureInfo.InvariantCulture, TimeStatsUrl, projectId, issueIid), cancellationToken);
        }

        public Task<Issue> CloneAsync(int projectId, int issueIid, IssueClone issueClone, CancellationToken cancellationToken = default)
        {
            return _api.Post().With(issueClone).ToAsync<Issue>(string.Format(CultureInfo.InvariantCulture, CloneIssueUrl, projectId, issueIid), cancellationToken);
        }

        private GitLabCollectionResponse<Issue> Get(string url, IssueQuery query)
        {
            url = AddIssueQueryParameters(url, query);
            return _api.Get().GetAllAsync<Issue>(url);
        }

        private static string AddIssueQueryParameters(string url, IssueQuery query)
        {
            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "issue_type", query.Type);
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "milestone", query.Milestone);
            url = Utils.AddParameter(url, "labels", query.Labels);
            url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
            url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
            url = Utils.AddParameter(url, "updated_after", query.UpdatedAfter);
            url = Utils.AddParameter(url, "updated_before", query.UpdatedBefore);
            url = Utils.AddParameter(url, "confidential", query.Confidential);
            url = Utils.AddParameter(url, "scope", query.Scope);
            url = Utils.AddParameter(url, "author_id", query.AuthorId);
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
            url = Utils.AddParameter(url, "search", query.Search);
            return url;
        }
    }
}
