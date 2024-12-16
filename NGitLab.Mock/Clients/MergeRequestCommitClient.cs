using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestCommitClient : ClientBase, IMergeRequestCommitClient
{
    private readonly long _projectId;
    private readonly long _mergeRequestIid;

    public MergeRequestCommitClient(ClientContext context, long projectId, long mergeRequestIid)
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
