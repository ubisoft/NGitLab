using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class CommitStatusClient : ClientBase, ICommitStatusClient
    {
        private readonly int _projectId;

        public CommitStatusClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public CommitStatusCreate AddOrUpdate(CommitStatusCreate status)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<CommitStatus> AllBySha(string commitSha)
        {
            throw new System.NotImplementedException();
        }
    }
}
