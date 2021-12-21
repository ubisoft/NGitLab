using System;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class CommitActionCreateHandler : ICommitActionHandler
    {
        public void Handle(CreateCommitAction action, string repoPath, LibGit2Sharp.Repository repository)
        {
            if (!Directory.Exists(repoPath))
                throw new DirectoryNotFoundException();

            var filePath = Path.Combine(repoPath, action.FilePath);

            if (System.IO.File.Exists(filePath))
                throw new GitLabException("File already exists.");

            if (string.Equals(action.Encoding, "base64", StringComparison.OrdinalIgnoreCase))
            {
                var content = Convert.FromBase64String(action.Content);
                System.IO.File.WriteAllBytes(filePath, content);
            }
            else
            {
                System.IO.File.WriteAllText(filePath, action.Content);
            }

            repository.Index.Add(action.FilePath);
            repository.Index.Write();
        }
    }
}
