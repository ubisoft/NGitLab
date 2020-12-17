using System.Collections.Generic;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestClient : IMergeRequestClient
    {
        private readonly API _api;
        private readonly int _projectId;
        private readonly string _projectPath;

        public MergeRequestClient(API api, int projectId)
        {
            _api = api;
            _projectId = projectId;
            _projectPath = Project.Url + "/" + projectId;
        }

        public MergeRequestClient(API api)
        {
            _api = api;
            _projectPath = string.Empty;
        }

        public IEnumerable<MergeRequest> All => Get(new MergeRequestQuery());

        public IEnumerable<MergeRequest> AllInState(MergeRequestState state) => Get(new MergeRequestQuery { State = state });

        public IEnumerable<MergeRequest> Get(MergeRequestQuery query)
        {
            var url = _projectPath + MergeRequest.Url;

            url = Utils.AddParameter(url, "state", query.State);
            url = Utils.AddParameter(url, "order_by", query.OrderBy);
            url = Utils.AddParameter(url, "sort", query.Sort);
            url = Utils.AddParameter(url, "milestone", query.Milestone);
            url = Utils.AddParameter(url, "view", query.View);
            url = Utils.AddParameter(url, "labels", query.Labels);
            url = Utils.AddParameter(url, "created_after", query.CreatedAfter);
            url = Utils.AddParameter(url, "created_before", query.CreatedBefore);
            url = Utils.AddParameter(url, "updated_after", query.UpdatedAfter);
            url = Utils.AddParameter(url, "updated_before", query.UpdatedBefore);
            url = Utils.AddParameter(url, "scope", query.Scope);
            url = Utils.AddParameter(url, "author_id", query.AuthorId);
            url = Utils.AddParameter(url, "per_page", query.PerPage);
            url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
            url = Utils.AddParameter(url, "approver_ids[]", query.ApproverIds);
            url = Utils.AddParameter(url, "source_branch", query.SourceBranch);
            url = Utils.AddParameter(url, "target_branch", query.TargetBranch);
            url = Utils.AddParameter(url, "search", query.Search);
            url = Utils.AddParameter(url, "wip", query.Wip.HasValue ? (query.Wip.Value ? "yes" : "no") : null);

            return _api.Get().GetAll<MergeRequest>(url);
        }

        public MergeRequest this[int iid]
        {
            get
            {
                var url = $"{_projectPath}{MergeRequest.Url}/{iid}";
                url = Utils.AddParameter(url, "include_rebase_in_progress", true);

                return _api.Get().To<MergeRequest>(url);
            }
        }

        public MergeRequest Create(MergeRequestCreate mergeRequest)
        {
            if (mergeRequest == null)
                throw new System.ArgumentNullException(nameof(mergeRequest));

            if (mergeRequest.TargetProjectId == null)
            {
                mergeRequest.TargetProjectId = _projectId;
            }

            return _api
                .Post().With(mergeRequest)
                .To<MergeRequest>(_projectPath + "/merge_requests");
        }

        public MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest) => _api
            .Put().With(mergeRequest)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

        public MergeRequest Close(int mergeRequestIid) => _api
            .Put().With(new MergeRequestUpdateState { NewState = nameof(MergeRequestStateEvent.close) })
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

        public MergeRequest Reopen(int mergeRequestIid) => _api
            .Put().With(new MergeRequestUpdateState { NewState = nameof(MergeRequestStateEvent.reopen) })
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture));

        public void Delete(int mergeRequestIid) => _api
            .Delete()
            .Execute(_projectPath + "/merge_requests/" + mergeRequestIid);

        public MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message) => _api
            .Put().With(message)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/merge");

        public MergeRequest Accept(int mergeRequestIid, MergeRequestMerge message) => _api
            .Put().With(message)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/merge");

        public MergeRequest Approve(int mergeRequestIid, MergeRequestApprove message) => _api
            .Post().With(message)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/approve");

        public RebaseResult Rebase(int mergeRequestIid) => _api
            .Put()
            .To<RebaseResult>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/rebase");

        public IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid)
        {
            return _api.Get().GetAll<PipelineBasic>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/pipelines");
        }

        public IEnumerable<Author> GetParticipants(int mergeRequestIid)
        {
            return _api.Get().GetAll<Author>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/participants");
        }

        public IEnumerable<Issue> ClosesIssues(int mergeRequestIid)
        {
            return _api.Get().GetAll<Issue>(_projectPath + "/merge_requests/" + mergeRequestIid.ToString(CultureInfo.InvariantCulture) + "/closes_issues");
        }

        public IMergeRequestCommentClient Comments(int mergeRequestIid) => new MergeRequestCommentClient(_api, _projectPath, mergeRequestIid);

        public IMergeRequestCommitClient Commits(int mergeRequestIid) => new MergeRequestCommitClient(_api, _projectPath, mergeRequestIid);

        public IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid) => new MergeRequestApprovalClient(_api, _projectPath, mergeRequestIid);
    }
}
