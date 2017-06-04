using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class MergeRequestCommitClient : IMergeRequestCommitClient
    {
        private readonly API _api;
        private readonly string _commitsPath;

        public MergeRequestCommitClient(API api, string projectPath, int mergeRequestId)
        {
            _api = api;
            _commitsPath = projectPath + "/merge_request/" + mergeRequestId + "/commits";
        }

        public IEnumerable<Commit> All()
        {
            return _api.Get().GetAll<Commit>(_commitsPath);
        }
    }
}