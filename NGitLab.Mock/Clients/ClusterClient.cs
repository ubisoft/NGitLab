using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ClusterClient : ClientBase, IClusterClient
{
    private readonly long _projectId;

    public ClusterClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public IEnumerable<ClusterInfo> All => throw new NotImplementedException();
}
