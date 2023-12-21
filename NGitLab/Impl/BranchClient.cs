using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl;

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

    public IEnumerable<Branch> Search(string search) =>
        _api.Get().GetAll<Branch>(_repoPath + "/branches?search=" + Uri.EscapeDataString(search));

    public GitLabCollectionResponse<Branch> SearchAsync(string search) =>
        _api.Get().GetAllAsync<Branch>(_repoPath + "/branches?search=" + Uri.EscapeDataString(search));

    public Branch this[string name] => _api.Get().To<Branch>(_repoPath + "/branches/" + Uri.EscapeDataString(name));

    public Branch Protect(string name) => _api.Put().To<Branch>(_repoPath + "/branches/" + Uri.EscapeDataString(name) + "/protect");

    public Branch Unprotect(string name) => _api.Put().To<Branch>(_repoPath + "/branches/" + Uri.EscapeDataString(name) + "/unprotect");

    public Branch Create(BranchCreate branch) => _api.Post().With(branch).To<Branch>(_repoPath + "/branches");

    public void Delete(string name) => _api.Delete().Execute(_repoPath + "/branches/" + Uri.EscapeDataString(name));
}
