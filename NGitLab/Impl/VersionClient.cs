using NGitLab.Models;

namespace NGitLab.Impl;

public class VersionClient : IVersionClient
{
    private readonly API _api;

    public VersionClient(API api)
    {
        _api = api;
    }

    public GitLabVersion Get() => _api.Get().To<GitLabVersion>("/version");
}
