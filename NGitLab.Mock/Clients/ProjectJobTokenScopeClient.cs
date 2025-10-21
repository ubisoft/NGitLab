using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectJobTokenScopeClient : ClientBase, IProjectJobTokenScopeClient
{
    private static Dictionary<long, JobTokenScope> _projectScopes = [];

    private readonly long _projectId;

    public ProjectJobTokenScopeClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
        lock (_projectScopes)
        {
            if (!_projectScopes.ContainsKey(_projectId))
            {
                _projectScopes[_projectId] = new JobTokenScope
                {
                    InboundEnabled = true,
                };
            }
        }
    }

    public async Task<JobTokenScope> GetProjectJobTokenScopeAsync(CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        lock (_projectScopes)
        {
            return _projectScopes[_projectId];
        }
    }

    public async Task UpdateProjectJobTokenScopeAsync(JobTokenScope scope, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        lock (_projectScopes)
        {
            _projectScopes[_projectId] = scope;
        }
    }
}
