using System.IO;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class CommitActionMoveHandler : ICommitActionHandler
    {
        public void Handle(CreateCommitAction action, string repoPath, LibGit2Sharp.Repository repository)
        {
            var oldPath = Path.Combine(repoPath, action.PreviousPath);
            if (!System.IO.File.Exists(oldPath))
                throw new FileNotFoundException("Could not find file", nameof(oldPath));

            var newPath = Path.Combine(repoPath, action.FilePath);
            System.IO.File.Move(oldPath, newPath);

            repository.Index.Add(action.FilePath);
            repository.Index.Add(action.PreviousPath);
            repository.Index.Write();
        }
    }
}
