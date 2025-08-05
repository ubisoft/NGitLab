using System;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Mock.Clients;

internal sealed class ClientContext
{
    private readonly SemaphoreSlim _operationLock = new SemaphoreSlim(1, 1);

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
        _operationLock.Wait();
        return new Releaser(_operationLock);
    }

    private sealed class Releaser : IDisposable
    {
        private readonly SemaphoreSlim _semaphore;

        public Releaser(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        public void Dispose()
        {
            _semaphore.Release();
        }
    }
}
