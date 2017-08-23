using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl {
    public class BranchClient : IBranchClient {
        readonly API _api;
        readonly string _repoPath;

        public BranchClient(API api, string repoPath) {
            _api = api;
            _repoPath = repoPath;
        }

        public IEnumerable<Branch> All() {
            return _api.Get().GetAll<Branch>(_repoPath + "/branches");
        }

        public Branch Get(string name) {
            return _api.Get().To<Branch>(_repoPath + "/branches/" + name);
        }

        public Branch Protect(string name) {
            return _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/protect");
        }

        public Branch Unprotect(string name) {
            return _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/unprotect");
        }

        public Branch Create(BranchCreate branch) {
            return _api.Post().With(branch).To<Branch>(_repoPath + "/branches");
        }

        public BranchRemovalStatus Delete(string name) {
            var errorMessage = _api.Delete().To<string>(_repoPath + "/branches/" + name);
            return BranchRemovalStatus.FromReponseMessage(errorMessage);
        }
    }
}