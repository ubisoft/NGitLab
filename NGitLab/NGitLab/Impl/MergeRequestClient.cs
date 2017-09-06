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

        public IEnumerable<MergeRequest> All => _api.Get().GetAll<MergeRequest>(_projectPath + "/merge_requests");

        public IEnumerable<MergeRequest> AllInState(MergeRequestState state) => _api.Get().GetAll<MergeRequest>(_projectPath + "/merge_requests?state=" + state);

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

        public MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message) => _api
            .Put().With(message)
            .To<MergeRequest>(_projectPath + "/merge_requests/" + mergeRequestIid + "/merge");

        public IMergeRequestCommentClient Comments(int mergeRequestIid) => new MergeRequestCommentClient(_api, _projectPath, mergeRequestIid);

        public IMergeRequestCommitClient Commits(int mergeRequestIid) => new MergeRequestCommitClient(_api, _projectPath, mergeRequestIid);
    }
}