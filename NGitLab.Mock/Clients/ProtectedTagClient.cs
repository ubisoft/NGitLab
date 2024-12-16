using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProtectedTagClient : ClientBase, IProtectedTagClient
{
    private readonly long _projectId;

    public ProtectedTagClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public ProtectedTag GetProtectedTag(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task<ProtectedTag> GetProtectedTagAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<ProtectedTag> GetProtectedTags()
    {
        throw new System.NotImplementedException();
    }

    public GitLabCollectionResponse<ProtectedTag> GetProtectedTagsAsync()
    {
        throw new System.NotImplementedException();
    }

    public ProtectedTag ProtectTag(TagProtect protect)
    {
        throw new System.NotImplementedException();
    }

    public Task<ProtectedTag> ProtectTagAsync(TagProtect protect, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }

    public void UnprotectTag(string name)
    {
        throw new System.NotImplementedException();
    }

    public Task UnprotectTagAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}
