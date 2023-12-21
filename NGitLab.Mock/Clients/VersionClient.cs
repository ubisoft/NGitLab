using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class VersionClient : ClientBase, IVersionClient
{
    public VersionClient(ClientContext context)
        : base(context)
    {
    }

    public GitLabVersion Get()
    {
        return Server.Version;
    }
}
