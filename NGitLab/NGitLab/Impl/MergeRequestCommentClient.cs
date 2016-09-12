using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestCommentClient : IMergeRequestCommentClient
    {
        private readonly API _api;
        private readonly string _commentsPath;

        public MergeRequestCommentClient(API api, string projectPath, int mergeRequestId)
        {
            _api = api;
            _commentsPath = projectPath + "/merge_request/" + mergeRequestId + "/comments";
        }

        public IEnumerable<MergeRequestComment> All => _api.Get().GetAll<MergeRequestComment>(_commentsPath);

        public MergeRequestComment Add(MergeRequestComment comment) => _api.Post().With(comment).To<MergeRequestComment>(_commentsPath);
    }
}