using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestClient
    {
        IEnumerable<MergeRequest> All { get; }

        IEnumerable<MergeRequest> AllInState(MergeRequestState state);

        IEnumerable<MergeRequest> Get(MergeRequestQuery query);

        MergeRequest this[int iid] { get; }

        Task<MergeRequest> GetByIidAsync(int iid, SingleMergeRequestQuery options, CancellationToken cancellationToken = default);

        MergeRequest Create(MergeRequestCreate mergeRequest);

        MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest);

        MergeRequest Close(int mergeRequestIid);

        MergeRequest Reopen(int mergeRequestIid);

        void Delete(int mergeRequestIid);

        MergeRequest CancelMergeWhenPipelineSucceeds(int mergeRequestIid);

        [Obsolete("You should use MergeRequestMerge instead of MergeRequestAccept")]
        MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message);

        MergeRequest Accept(int mergeRequestIid, MergeRequestMerge message);

        MergeRequest Approve(int mergeRequestIid, MergeRequestApprove message);

        RebaseResult Rebase(int mergeRequestIid);

        Task<RebaseResult> RebaseAsync(int mergeRequestIid, MergeRequestRebase options, CancellationToken cancellationToken = default);

        IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid);

        IEnumerable<Author> GetParticipants(int mergeRequestIid);

        IMergeRequestCommentClient Comments(int mergeRequestIid);

        IMergeRequestDiscussionClient Discussions(int mergeRequestIid);

        IMergeRequestCommitClient Commits(int mergeRequestIid);

        IMergeRequestChangeClient Changes(int mergeRequestIid);

        IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid);

        IEnumerable<Issue> ClosesIssues(int mergeRequestIid);
    }
}
