using NGitLab.Models;

namespace NGitLab.Impl {
    public class MergeRequestChangesClient : IMergeRequestChangesClient {
        readonly Api api;
        readonly string changesPath;
        public MergeRequestChanges Changes => api.Get().To<MergeRequestChanges>(changesPath);
        public MergeRequestChangesClient(Api api, string projectPath, int mergeRequestId) {
            this.api = api;
            this.changesPath = projectPath + "/merge_requests/" + mergeRequestId + "/changes";
        }
    }
}