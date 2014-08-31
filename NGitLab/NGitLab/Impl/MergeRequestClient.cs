using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestClient : IMergeRequestClient
    {
        private readonly API _api;
        private readonly string _projectPath;

        public MergeRequestClient(API api, int projectId)
        {
            _api = api;
            _projectPath = Project.Url + "/" + projectId;
        }

        public IEnumerable<MergeRequest> All
        {
            get { return _api.Get().GetAll<MergeRequest>(_projectPath + "/merge_requests"); }
        }

        public IEnumerable<MergeRequest> AllInState(MergeRequestState state)
        {
            return _api.Get().GetAll<MergeRequest>(_projectPath + "/merge_requests?state=" + state);
        }

        public MergeRequest this[int id]
        {
            get { return _api.Get().To<MergeRequest>(_projectPath + "/merge_request/" + id); }
        }

        public MergeRequest Create(MergeRequestCreate mergeRequest)
        {
            return _api
                .Post().With(mergeRequest)
                .To<MergeRequest>(_projectPath + "/merge_requests");
        }

        public MergeRequest Update(int mergeRequestId, MergeRequestUpdate mergeRequest)
        {
            return _api
                .Put().With(mergeRequest)
                .To<MergeRequest>(_projectPath + "/merge_request/" + mergeRequestId);
        }

        public MergeRequest Accept(int mergeRequestId, MergeCommitMessage message)
        {
            return _api
                .Put().With(message)
                .To<MergeRequest>(_projectPath + "/merge_request/" + mergeRequestId + "/merge");
        }

        public IMergeRequestCommentClient Comments(int mergeRequestId)
        {
            return new MergeRequestCommentClient(_api, _projectPath, mergeRequestId);
        }        
    }
}