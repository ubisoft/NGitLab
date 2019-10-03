using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<Commit> All
        {
            get
            {
                var mergeRequest = GetMergeRequest(_projectId, _mergeRequestIid);
                return mergeRequest.Commits.Select(commit => commit.ToCommitClient());
            }
        }
    }
}
