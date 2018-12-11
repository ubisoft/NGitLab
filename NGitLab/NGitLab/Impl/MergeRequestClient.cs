using System.Collections.Generic;
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
            _projectPath = "";
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
            if (query.AssigneeId == 0) // unassigned. In the next version of GitLab, 0 or empty mean unassigned, but in the current version we must use an empty value.
            {
                url = Utils.AddParameter(url, "assignee_id", "");
            }
            else
            {
                url = Utils.AddParameter(url, "assignee_id", query.AssigneeId);
            }
            url = Utils.AddParameter(url, "source_branch", query.SourceBranch);
            url = Utils.AddParameter(url, "target_branch", query.TargetBranch);
            url = Utils.AddParameter(url, "search", query.Search);

            return _api.Get().GetAll<MergeRequest>(url);
        }

        public MergeRequest this[int iid] => _api.Get().To<MergeRequest>(_projectPath + "/merge_requests/" + iid);

        public MergeRequest Create(MergeRequestCreate mergeRequest)
        {
            if (mergeRequest == null)
                throw new System.ArgumentNullException(nameof(mergeRequest));

            if(mergeRequest.TargetProjectId == null)
            {
                mergeRequest.TargetProjectId = _projectId;
            }

            return _api
                .Post().With(mergeRequest)
                .To<MergeRequest>(_projectPath + "/merge_requests");
        }

        public MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest) => _api
            .Put().With(mergeRequest)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid);

        public MergeRequest Close(int mergeRequestIid) => _api
            .Put().With(new MergeRequestUpdateState {NewState = MergeRequestStateEvent.close.ToString()})
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid);

        public MergeRequest Reopen(int mergeRequestIid) => _api
            .Put().With(new MergeRequestUpdateState { NewState = MergeRequestStateEvent.reopen.ToString() })
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid);


        public void Delete(int mergeRequestIid) => _api
            .Delete()
            .Execute(_projectPath + "/merge_requests/" + mergeRequestIid);

        public MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message) => _api
            .Put().With(message)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid + "/merge");

        public IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid)
        {
            return _api.Get().GetAll<PipelineBasic>(_projectPath + "/merge_requests/" + mergeRequestIid + "/pipelines");
        }

        public IMergeRequestCommentClient Comments(int mergeRequestIid) => new MergeRequestCommentClient(_api, _projectPath, mergeRequestIid);

        public IMergeRequestCommitClient Commits(int mergeRequestIid) => new MergeRequestCommitClient(_api, _projectPath, mergeRequestIid);

        public IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid) => new MergeRequestApprovalClient(_api, _projectPath, mergeRequestIid);
    }
}
