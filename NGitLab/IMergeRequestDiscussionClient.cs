using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestDiscussionClient
    {
        IEnumerable<MergeRequestDiscussion> All { get; }

        MergeRequestDiscussion Add(MergeRequestDiscussionCreate discussion);

        MergeRequestDiscussion Resolve(MergeRequestDiscussionResolve resolve);

        void Delete(string discussionId, long commentId);
    }
}
