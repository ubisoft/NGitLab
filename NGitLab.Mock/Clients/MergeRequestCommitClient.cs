using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class MergeRequestCommitClient : ClientBase, IMergeRequestCommitClient
    {
        private readonly int _projectId;
        private readonly int _mergeRequestIid;

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
                using (Context.BeginOperationScope())
                {
                    var mergeRequest = GetMergeRequest(_projectId, _mergeRequestIid);
                    return mergeRequest.Commits.Select(commit => commit.ToCommitClient(mergeRequest.SourceProject)).ToList();
                }
            }
        }
    }
}
