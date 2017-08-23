using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class BranchClient : IBranchClient {
        readonly Api api;
        readonly string repoPath;

        public BranchClient(Api api, string repoPath) {
            this.api = api;
            this.repoPath = repoPath;
        }

        public IEnumerable<Branch> All() {
            return api.Get().GetAll<Branch>(repoPath + "/branches");
        }

        public Branch Get(string name) {
            return api.Get().To<Branch>(repoPath + "/branches/" + name);
        }

        public Branch Protect(string name) {
            return api.Put().To<Branch>(repoPath + "/branches/" + name + "/protect");
        }

        public Branch Unprotect(string name) {
            return api.Put().To<Branch>(repoPath + "/branches/" + name + "/unprotect");
        }

        public Branch Create(BranchCreate branch) {
            return api.Post().With(branch).To<Branch>(repoPath + "/branches");
        }

        public BranchRemovalStatus Delete(string name) {
            var errorMessage = api.Delete().To<string>(repoPath + "/branches/" + name);
            return BranchRemovalStatus.FromReponseMessage(errorMessage);
        }
    }
}