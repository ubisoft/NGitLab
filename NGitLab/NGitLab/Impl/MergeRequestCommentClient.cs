using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class MergeRequestCommentClient : IMergeRequestCommentClient {
        readonly Api api;
        readonly string commentsPath;
        public MergeRequestCommentClient(Api api, string projectPath, int mergeRequestId) {
            this.api = api;
            commentsPath = projectPath + "/merge_requests/" + mergeRequestId + "/notes";
        }

        public IEnumerable<Comment> All => api.Get().GetAll<Comment>(commentsPath);


        public Comment Add(MergeRequestComment comment) {
            return api.Post().With(comment).To<Comment>(commentsPath);
        }
    }
}