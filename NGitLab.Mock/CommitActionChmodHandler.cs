#if NET7_0_OR_GREATER
using System;
#endif

using System.IO;
using LibGit2Sharp;
using NGitLab.Models;

namespace NGitLab.Mock;

internal sealed class CommitActionChmodHandler : ICommitActionHandler
{
    public void Handle(CreateCommitAction action, string repoPath, LibGit2Sharp.Repository repository)
    {
        if (!Directory.Exists(repoPath))
            throw new DirectoryNotFoundException();

        var filePath = Path.Combine(repoPath, action.FilePath);

        if (!System.IO.File.Exists(filePath))
            throw new FileNotFoundException("File does not exist.");

#if NET7_0_OR_GREATER
        if (!OperatingSystem.IsWindows())
        {
            var fileMode = UnixFileMode.UserRead | UnixFileMode.UserWrite |
                           UnixFileMode.GroupRead | UnixFileMode.GroupWrite |
                           UnixFileMode.OtherRead | UnixFileMode.OtherWrite |
                           (action.IsExecutable
                               ? UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute
                               : UnixFileMode.None);
            System.IO.File.SetUnixFileMode(filePath, fileMode);
        }
#endif

        var blob = repository.ObjectDatabase.CreateBlob(filePath);
        repository.Index.Add(blob, action.FilePath, action.IsExecutable ? Mode.ExecutableFile : Mode.NonExecutableFile);
        repository.Index.Write();
    }
}
