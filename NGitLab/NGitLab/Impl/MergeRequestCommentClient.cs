using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class MergeRequestCommentClient : IMergeRequestCommentClient {
        readonly API _api;
        readonly string _commentsPath;

        public MergeRequestCommentClient(API api, string projectPath, int mergeRequestId) {
            _api = api;
            _commentsPath = projectPath + "/merge_request/" + mergeRequestId + "/comments";
        }

        public IEnumerable<Comment> All() {
            return _api.Get().GetAll<Comment>(_commentsPath);
        }

        public Comment Add(MergeRequestComment comment) {
            return _api.Post().With(comment).To<Comment>(_commentsPath);
        }
    }
}