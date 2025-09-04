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
        [CallerMemberName] string memberName = null,
        [CallerFilePath] string sourceFilePath = null,
        [CallerLineNumber] int sourceLineNumber = -1)
    {
        Server.RaiseOnClientOperation();
        var releaser = new Releaser(_operationLock);

        // Store caller info for debugging purposes
        Releaser.CallerMemberName = memberName;
        Releaser.CallerFilePath = sourceFilePath;
        Releaser.SourceLineNumber = sourceLineNumber;

        return releaser;
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

        // The following is for debugging purposes only. It stores the caller info of the current operation scope.
        public static string CallerMemberName { get; set; }

        public static string CallerFilePath { get; set; }

        public static int SourceLineNumber { get; set; }

        public void Dispose()
        {
            _operationLock.Release();
        }
    }
}
