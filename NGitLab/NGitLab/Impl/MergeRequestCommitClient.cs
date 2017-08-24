using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class MergeRequestCommitClient : IMergeRequestCommitClient {
        readonly Api api;
        readonly string commitsPath;

        public MergeRequestCommitClient(Api api, string projectPath, int mergeRequestId) {
            this.api = api;
            commitsPath = projectPath + "/merge_requests/" + mergeRequestId + "/commits";
        }

        public IEnumerable<Commit> All() {
            return api.Get().GetAll<Commit>(commitsPath);
        }
    }
}