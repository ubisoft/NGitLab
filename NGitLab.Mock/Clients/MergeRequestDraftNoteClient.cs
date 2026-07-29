using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class MergeRequestDraftNoteClient : ClientBase, IMergeRequestDraftNoteClient
{
    public MergeRequestDraftNoteClient(ClientContext context)
        : base(context)
    {
    }

    public IEnumerable<DraftNote> All => throw new NotImplementedException();

    public Task<DraftNote> CreateAsync(DraftNoteCreate draftNote, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task PublishAllAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
