using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class MergeRequestClient : IMergeRequestClient {
        readonly Api api;
        readonly string projectPath;

        public MergeRequestClient(Api api, int projectId) {
            this.api = api;
            projectPath = Project.Url + "/" + projectId;
        }

        public IEnumerable<MergeRequest> All() {
            return api.Get().GetAll<MergeRequest>(projectPath + "/merge_requests");
        }

        public IEnumerable<MergeRequest> AllInState(MergeRequestState state) {
            return api.Get().GetAll<MergeRequest>(projectPath + "/merge_requests?state=" + state);
        }

        public MergeRequest Get(int id) {
            return api.Get().To<MergeRequest>(projectPath + "/merge_requests/" + id);
        }

        public MergeRequest Create(MergeRequestCreate mergeRequest) {
            return api
                .Post().With(mergeRequest)
                .To<MergeRequest>(projectPath + "/merge_requests");
        }

        public MergeRequest Update(int mergeRequestId, MergeRequestUpdate mergeRequest) {
            return api
                .Put().With(mergeRequest)
                .To<MergeRequest>(projectPath + "/merge_requests/" + mergeRequestId);
        }

        public MergeRequest Accept(int mergeRequestId, MergeCommitMessage message) {
            return api
                .Put().With(message)
                .To<MergeRequest>(projectPath + "/merge_requests/" + mergeRequestId + "/merge");
        }

        public IMergeRequestCommentClient Comments(int mergeRequestId) {
            return new MergeRequestCommentClient(api, projectPath, mergeRequestId);
        }

        public IMergeRequestCommitClient Commits(int mergeRequestId) {
            return new MergeRequestCommitClient(api, projectPath, mergeRequestId);
        }

        public IMergeRequestChangesClient Changes(int mergeRequestId) {
            return new MergeRequestChangesClient(api, projectPath, mergeRequestId);
        }
    }
}