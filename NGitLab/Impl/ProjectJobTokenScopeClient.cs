using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public sealed class ProjectJobTokenScopeClient : IProjectJobTokenScopeClient
{
    private readonly API _api;
    private readonly string _jobTokenScopeUrl;

    internal ProjectJobTokenScopeClient(API api, ProjectId projectId)
    {
        _api = api;
        _jobTokenScopeUrl = $"{Project.Url}/{projectId.ValueAsUriParameter()}/job_token_scope";
    }

    public Task<JobTokenScope> GetProjectJobTokenScope(CancellationToken cancellationToken = default)
    {
        return _api.Get().ToAsync<JobTokenScope>(_jobTokenScopeUrl, cancellationToken);
    }

    public async Task UpdateProjectJobTokenScope(JobTokenScope scope, CancellationToken cancellationToken = default)
    {
        await _api.Patch().With(new { enabled = scope.InboundEnabled }).ExecuteAsync(_jobTokenScopeUrl, cancellationToken).ConfigureAwait(false);
    }
}
