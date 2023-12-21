using System;
using System.Threading;

namespace NGitLab.Mock.Clients;

internal sealed class ClientContext
{
    private readonly object _operationLock = new();

    public ClientContext(GitLabServer server, User user)
    {
        Server = server;
        User = user;
    }

    public GitLabServer Server { get; }

    public User User { get; }

    public bool IsAuthenticated => User != null;

    public IDisposable BeginOperationScope()
    {
        Server.RaiseOnClientOperation();
        Monitor.Enter(_operationLock);
        return new Releaser(_operationLock);
    }

    private sealed class Releaser : IDisposable
    {
        private readonly object _operationLock;

        public Releaser(object operationLock)
        {
            _operationLock = operationLock;
        }

        public void Dispose()
        {
            Monitor.Exit(_operationLock);
        }
    }
}
