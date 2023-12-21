using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class  MergeRequestChangeClient : ClientBase, IMergeRequestChangeClient
{
    private readonly int _projectId;
    private readonly int _mergeRequestIid;

    public MergeRequestChangeClient(ClientContext context, int projectId, int mergeRequestIid)
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
