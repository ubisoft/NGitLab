using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IMergeRequestDiscussionClient
{
    IEnumerable<MergeRequestDiscussion> All { get; }

    MergeRequestDiscussion Get(string id);

    Task<MergeRequestDiscussion> GetAsync(string id, CancellationToken cancellationToken = default);

    MergeRequestDiscussion Add(MergeRequestDiscussionCreate discussion);

    MergeRequestDiscussion Resolve(MergeRequestDiscussionResolve resolve);

    void Delete(string discussionId, long commentId);
}
