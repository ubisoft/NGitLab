using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestChangeClient : ClientBase, IMergeRequestChangeClient
{
    private readonly long _projectId;
    private readonly long _mergeRequestIid;

    public MergeRequestChangeClient(ClientContext context, long projectId, long mergeRequestIid)
        : base(context)
    {
        _projectId = projectId;
        _mergeRequestIid = mergeRequestIid;
    }

    public MergeRequestChange MergeRequestChange
    {
        get
        {
            using (Context.BeginOperationScope())
            {
                return new MergeRequestChange
                {
                    Changes = GetMergeRequest(_projectId, _mergeRequestIid).Changes.Select(a => a.ToChange())
                        .ToArray(),
                };
            }
        }
    }
}
