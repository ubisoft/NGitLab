using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Impl;

public class MergeRequestDraftNoteClient : IMergeRequestDraftNoteClient
{
    private readonly API _api;
    private readonly string _draftNotesPath;
    private readonly string _bulkPublishPath;

    public MergeRequestDraftNoteClient(API api, string projectPath, long mergeRequestIid)
    {
        _api = api;
        var iid = mergeRequestIid.ToString(CultureInfo.InvariantCulture);
        _draftNotesPath = projectPath + "/merge_requests/" + iid + "/draft_notes";
        _bulkPublishPath = _draftNotesPath + "/bulk_publish";
    }

    public IEnumerable<DraftNote> All => _api.Get().GetAll<DraftNote>(_draftNotesPath);

    public Task<DraftNote> CreateAsync(DraftNoteCreate draftNote, CancellationToken cancellationToken = default)
        => _api.Post().With(draftNote).ToAsync<DraftNote>(_draftNotesPath, cancellationToken);

    public Task PublishAllAsync(CancellationToken cancellationToken = default)
        => _api.Post().ExecuteAsync(_bulkPublishPath, cancellationToken);
}
