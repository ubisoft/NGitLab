using System;
using NeoSmart.AsyncLock;

namespace NGitLab.Mock.Clients;

internal sealed class ClientContext(GitLabServer server, User user)
{
    private readonly AsyncLock _operationLock = new();

    public GitLabServer Server { get; } = server;

    public User User { get; } = user;

    public bool IsAuthenticated => User is not null;

    public IDisposable BeginOperationScope()
    {
        Server.RaiseOnClientOperation();
        return _operationLock.Lock();
    }
}
