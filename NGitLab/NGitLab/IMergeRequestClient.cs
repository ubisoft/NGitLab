using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestClient
    {
        IEnumerable<MergeRequest> All { get; }
        IEnumerable<MergeRequest> AllInState(MergeRequestState state);
        MergeRequest this[int iid] { get; }

        MergeRequest Create(MergeRequestCreate mergeRequest);
        MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest);
        MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message);

        IMergeRequestCommentClient Comments(int mergeRequestIid);
        IMergeRequestCommitClient Commits(int mergeRequestIid);
    }
}