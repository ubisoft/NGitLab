using NGitLab.Models;

namespace NGitLab.Mock
{
    internal interface ICommitActionHandler
    {
        void Handle(CreateCommitAction action, string repoPath, LibGit2Sharp.Repository repository);
    }
}
