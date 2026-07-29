using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class MergeRequestDraftNoteClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task CreateAsync_adds_draft_note_visible_in_All()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var draftNoteClient = context.Client.GetMergeRequest(project.Id).DraftNotes(mergeRequest.Iid);

        var created = await draftNoteClient.CreateAsync(new DraftNoteCreate { Note = "Draft review comment" });

        Assert.That(created, Is.Not.Null);
        Assert.That(created.Note, Is.EqualTo("Draft review comment"));
        Assert.That(created.Id, Is.GreaterThan(0));

        var all = draftNoteClient.All.ToList();
        Assert.That(all, Has.Count.EqualTo(1), "The created draft note should appear in All");
        Assert.That(all[0].Id, Is.EqualTo(created.Id));
    }

    [Test]
    [NGitLabRetry]
    public async Task All_returns_empty_when_no_draft_notes_exist()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var draftNoteClient = context.Client.GetMergeRequest(project.Id).DraftNotes(mergeRequest.Iid);

        var all = draftNoteClient.All.ToList();

        Assert.That(all, Is.Empty, "No draft notes should exist on a fresh merge request");
    }

    [Test]
    [NGitLabRetry]
    public async Task PublishAllAsync_publishes_all_draft_notes()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var (project, mergeRequest) = context.CreateMergeRequest();
        var mrClient = context.Client.GetMergeRequest(project.Id);
        var draftNoteClient = mrClient.DraftNotes(mergeRequest.Iid);

        await draftNoteClient.CreateAsync(new DraftNoteCreate { Note = "First draft" });
        await draftNoteClient.CreateAsync(new DraftNoteCreate { Note = "Second draft" });

        Assert.That(draftNoteClient.All.Count(), Is.EqualTo(2), "Two draft notes should exist before publishing");

        await draftNoteClient.PublishAllAsync();

        // After publishing, draft notes should be cleared
        var remaining = draftNoteClient.All.ToList();
        Assert.That(remaining, Is.Empty, "Draft notes should be gone after PublishAllAsync");

        // The notes should now appear as regular MR notes/discussions
        var notes = mrClient.Comments(mergeRequest.Iid).All.ToList();
        Assert.That(notes.Any(n => string.Equals(n.Body, "First draft", System.StringComparison.Ordinal)
                                || string.Equals(n.Body, "Second draft", System.StringComparison.Ordinal)), Is.True,
            "Published draft notes should appear as regular comments");
    }
}
