using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

/// <summary>
/// Client for the GitLab draft notes API:
/// <c>GET/POST /projects/:id/merge_requests/:iid/draft_notes</c> and
/// <c>POST /projects/:id/merge_requests/:iid/draft_notes/bulk_publish</c>.
/// </summary>
public interface IMergeRequestDraftNoteClient
{
    IEnumerable<DraftNote> All { get; }

    Task<DraftNote> CreateAsync(DraftNoteCreate draftNote, CancellationToken cancellationToken = default);

    /// <summary>
    /// Publishes all draft notes for the merge request in a single request.
    /// </summary>
    Task PublishAllAsync(CancellationToken cancellationToken = default);
}
