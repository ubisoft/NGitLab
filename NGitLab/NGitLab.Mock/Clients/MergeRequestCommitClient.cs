using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestCommitClient : ClientBase, IMergeRequestCommitClient
    {
        private int _projectId;
        private int _mergeRequestIid;

        public MergeRequestCommitClient(ClientContext context, int projectId, int mergeRequestIid)
            : base(context)
        {
            _projectId = projectId;
            _mergeRequestIid = mergeRequestIid;
        }

        public IEnumerable<Commit> All => throw new NotImplementedException();
    }
}
