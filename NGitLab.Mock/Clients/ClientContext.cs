using System;
using System.Diagnostics;
using System.Threading;

namespace NGitLab.Mock.Clients;

internal sealed class ClientContext(GitLabServer server, User user)
{
    private readonly SemaphoreSlim _operationLock = new(1, 1);

    public GitLabServer Server { get; } = server;

    public User User { get; } = user;

    public bool IsAuthenticated => User != null;

    public IDisposable BeginOperationScope()
    {
        Server.RaiseOnClientOperation();
        return new Releaser(_operationLock);
    }

    private sealed class Releaser : IDisposable
    {
        private readonly SemaphoreSlim _operationLock;

        public Releaser(SemaphoreSlim operationLock)
        {
            _operationLock = operationLock;
            if (Debugger.IsAttached && _operationLock.CurrentCount == 0)
                Debugger.Break();
            _operationLock.Wait();
        }

        public void Dispose()
        {
            _operationLock.Release();
        }
    }
}
