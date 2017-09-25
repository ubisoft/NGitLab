using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return api.Get().GetAll<PipelineData>(repoPath + "/pipelines");
        }
        public Pipeline Get(int id) {
            return api.Get().To<Pipeline>(pipelinesPath + $@"/{id}");
        }
    }
}
