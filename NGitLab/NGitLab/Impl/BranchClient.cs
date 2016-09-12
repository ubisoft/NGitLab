using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class BranchClient : IBranchClient
    {
        private readonly API _api;
        private readonly string _repoPath;

        public BranchClient(API api, string repoPath)
        {
            _api = api;
            _repoPath = repoPath;
        }

        public IEnumerable<Branch> All => _api.Get().GetAll<Branch>(_repoPath + "/branches");

        public Branch this[string name] => _api.Get().To<Branch>(_repoPath + "/branches/" + name);

        public Branch Protect(string name) => _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/protect");

        public Branch Unprotect(string name) => _api.Put().To<Branch>(_repoPath + "/branches/" + name + "/unprotect");

        public Branch Create(BranchCreate branch) => _api.Post().With(branch).To<Branch>(_repoPath + "/branches");

        public BranchRemovalStatus Delete(string name) => BranchRemovalStatus.FromReponseMessage(_api.Delete().To<string>(_repoPath + "/branches/" + name));
    }
}