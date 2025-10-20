using System;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectJobTokenScopeClient : ClientBase, IProjectJobTokenScopeClient
{
    private readonly long _projectId;

    public ProjectJobTokenScopeClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Task<JobTokenScope> GetProjectJobTokenScopeAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProjectJobTokenScopeAsync(JobTokenScope scope, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
