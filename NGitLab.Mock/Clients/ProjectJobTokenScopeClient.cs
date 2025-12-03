using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectJobTokenScopeClient : ClientBase, IProjectJobTokenScopeClient
{
    private static readonly Dictionary<long, JobTokenScope> s_projectScopes = [];

    private readonly long _projectId;

    public ProjectJobTokenScopeClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
        lock (s_projectScopes)
        {
            if (!s_projectScopes.ContainsKey(_projectId))
            {
                s_projectScopes[_projectId] = new JobTokenScope
                {
                    InboundEnabled = true,
                };
            }
        }
    }

    public async Task<JobTokenScope> GetProjectJobTokenScopeAsync(CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        lock (s_projectScopes)
        {
            return s_projectScopes[_projectId];
        }
    }

    public async Task UpdateProjectJobTokenScopeAsync(JobTokenScope scope, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        lock (s_projectScopes)
        {
            s_projectScopes[_projectId] = scope;
        }
    }
}
