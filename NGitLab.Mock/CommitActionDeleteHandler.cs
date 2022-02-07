using System.IO;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class CommitActionDeleteHandler : ICommitActionHandler
    {
        public void Handle(CreateCommitAction action, string repoPath, LibGit2Sharp.Repository repository)
        {
            var fullPath = Path.Combine(repoPath, action.FilePath);

            if (!System.IO.File.Exists(fullPath))
                throw new FileNotFoundException("Could not find file", fullPath);

            System.IO.File.Delete(fullPath);

            repository.Index.Remove(action.FilePath);
            repository.Index.Write();
        }
    }
}
