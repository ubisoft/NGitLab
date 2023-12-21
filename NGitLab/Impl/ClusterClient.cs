using System.Collections.Generic;
using System.ComponentModel;
using NGitLab.Models;

namespace NGitLab.Impl;

public class ClusterClient : IClusterClient
{
    private readonly API _api;
    private readonly string _environmentsPath;

    [EditorBrowsable(EditorBrowsableState.Never)]
    public ClusterClient(API api, int projectId)
        : this(api, (long)projectId)
    {
    }

    public ClusterClient(API api, ProjectId projectId)
    {
        _api = api;
        _environmentsPath = $"{Project.Url}/{projectId.ValueAsUriParameter()}/clusters";
    }

    public IEnumerable<ClusterInfo> All => _api.Get().GetAll<ClusterInfo>(_environmentsPath);
}
