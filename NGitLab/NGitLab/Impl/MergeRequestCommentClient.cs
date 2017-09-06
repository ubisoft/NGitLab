using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestCommentClient : IMergeRequestCommentClient
    {
        private readonly API _api;
        private readonly string _notesPath;

        public MergeRequestCommentClient(API api, string projectPath, int mergeRequestIid)
        {
            _api = api;
            _notesPath = projectPath + "/merge_requests/" + mergeRequestIid + "/notes";
        }

        public IEnumerable<MergeRequestComment> All => _api.Get().GetAll<MergeRequestComment>(_notesPath);

        public MergeRequestComment Add(MergeRequestComment comment) => _api.Post().With(comment).To<MergeRequestComment>(_notesPath);
    }
}