using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NGitLab.Mock.Clients;

internal sealed class ClientContext(GitLabServer server, User user)
{
    private readonly SemaphoreSlim _operationLock = new(1, 1);

    public GitLabServer Server { get; } = server;

    public User User { get; } = user;

    public bool IsAuthenticated => User != null;

    public IDisposable BeginOperationScope(
        [CallerMemberName] string callingMethod = null,
        [CallerFilePath] string callingFilePath = null,
        [CallerLineNumber] int callingLineNumber = -1)
    {
        Server.RaiseOnClientOperation();
        var releaser = new Releaser(_operationLock);

        // Store caller info for debugging purposes
        Releaser.MethodWhereLockWasTaken = callingMethod;
        Releaser.FilePathWhereLockWasTaken = callingFilePath;
        Releaser.LineNumberWhereLockWasTaken = callingLineNumber;

        return releaser;
    }

    private sealed class Releaser : IDisposable
    {
        private readonly SemaphoreSlim _operationLock;

        public Releaser(SemaphoreSlim operationLock)
        {
            _operationLock = operationLock;
            if (_operationLock.CurrentCount == 0 && Debugger.IsAttached)
                Debugger.Break();
            _operationLock.Wait();
        }

        // The following is for debugging purposes only. It stores info about where the active lock was taken.
        public static string MethodWhereLockWasTaken { get; set; }

        public static string FilePathWhereLockWasTaken { get; set; }

        public static int LineNumberWhereLockWasTaken { get; set; }

        public void Dispose()
        {
            _operationLock.Release();
        }
    }
}
