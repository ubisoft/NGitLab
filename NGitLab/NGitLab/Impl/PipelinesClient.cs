using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class PipelinesClient : IPipelinesClient {
        readonly Api api;
        readonly string repoPath;
        readonly string pipelinesPath;

        public PipelinesClient(Api api, string repoPath) {
            this.api = api;
            this.repoPath = repoPath;
            this.pipelinesPath = repoPath + "/pipelines";
        }
        public IEnumerable<PipelineData> All() {
            return api.Get().GetAll<PipelineData>(pipelinesPath);
        }
        public Pipeline Get(int id) {
            return api.Get().To<Pipeline>(pipelinesPath + $@"/{id}");
        }
        public IEnumerable<Job> GetJobs(int id) {
            return api.Get().GetAll<Job>(pipelinesPath + $@"/{id}/jobs");
        }
    }
}
