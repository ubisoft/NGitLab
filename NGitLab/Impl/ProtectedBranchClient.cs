using System;
using NGitLab.Models;

namespace NGitLab.Impl;

internal sealed class ProtectedBranchClient : IProtectedBranchClient
{
    private readonly API _api;
    private readonly string _protectedBranchesUrl;

    public ProtectedBranchClient(API api, ProjectId projectId)
    {
        _api = api;
        _protectedBranchesUrl = $"{Project.Url}/{projectId.ValueAsUriParameter()}/protected_branches";
    }

    public ProtectedBranch ProtectBranch(BranchProtect branchProtect)
        => _api.Post().With(branchProtect).To<ProtectedBranch>(_protectedBranchesUrl);

    public void UnprotectBranch(string branchName)
        => _api.Delete().Execute($"{_protectedBranchesUrl}/{Uri.EscapeDataString(branchName)}");

    public ProtectedBranch GetProtectedBranch(string branchName)
        => _api.Get().To<ProtectedBranch>($"{_protectedBranchesUrl}/{Uri.EscapeDataString(branchName)}");

    public ProtectedBranch[] GetProtectedBranches(string search = null)
        => _api.Get().To<ProtectedBranch[]>(Utils.AddParameter(_protectedBranchesUrl, "search", search));
}
