using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab {
    public interface IMergeRequestClient {
        IEnumerable<MergeRequest> All();
        IEnumerable<MergeRequest> AllInState(MergeRequestState state);
        MergeRequest Get(int id);

        MergeRequest Create(MergeRequestCreate mergeRequest);
        MergeRequest Update(int mergeRequestId, MergeRequestUpdate mergeRequest);
        MergeRequest Accept(int mergeRequestId, MergeCommitMessage message);

        IMergeRequestCommentClient Comments(int mergeRequestId);
        IMergeRequestCommitClient Commits(int mergeRequestId);
        IMergeRequestChangesClient Changes(int mergeRequestId);
    }
}