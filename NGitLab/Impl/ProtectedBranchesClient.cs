using NGitLab.Models;

namespace NGitLab.Impl
{
    internal sealed class ProtectedBranchesClient : IProtectedBranchesClient
    {
        private readonly API _api;
        private readonly int _projectId;
        private readonly string _protectedBranchesUrl;

        public ProtectedBranchesClient(API api, int projectId)
        {
            _api = api;
            _projectId = projectId;
            _protectedBranchesUrl = $"{Project.Url}/{_projectId}/protected_branches";
        }

        public ProtectedBranch ProtectBranch(BranchProtect branchProtect)
            => _api.Post().With(branchProtect).To<ProtectedBranch>(_protectedBranchesUrl);

        public void UnprotectBranch(string branchName)
            => _api.Delete().Execute($"{_protectedBranchesUrl}/{branchName}");

        public ProtectedBranch GetProtectedBranch(string branchName)
            => _api.Get().To<ProtectedBranch>($"{_protectedBranchesUrl}/{Uri.EscapeDataString(branchName)}");

        public ProtectedBranch GetProtectedBranches(string search)
        {
            var searchParameter = string.IsNullOrWhiteSpace(search) ? string.Empty : $"/search={search}";

            return _api.Get().To<ProtectedBranch>($"{_protectedBranchesUrl}{searchParameter}");
        }
    }
}
