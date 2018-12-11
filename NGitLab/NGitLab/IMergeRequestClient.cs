using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestClient
    {
        IEnumerable<MergeRequest> All { get; }
        IEnumerable<MergeRequest> AllInState(MergeRequestState state);
        IEnumerable<MergeRequest> Get(MergeRequestQuery query);

        MergeRequest this[int iid] { get; }

        MergeRequest Create(MergeRequestCreate mergeRequest);
        MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest);
        MergeRequest Close(int mergeRequestIid);
        MergeRequest Reopen(int mergeRequestIid);
        void Delete(int mergeRequestIid);
        MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message);
        IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid);
        IMergeRequestCommentClient Comments(int mergeRequestIid);
        IMergeRequestCommitClient Commits(int mergeRequestIid);
        IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid);
    }
}
