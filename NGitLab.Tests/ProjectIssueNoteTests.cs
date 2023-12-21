using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class ProjectIssueNoteTests
{
    [Test]
    [NGitLabRetry]
    public async Task CreateNote()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issue = context.Client.Issues.Create(new IssueCreate { ProjectId = project.Id, Title = "test", Description = "test" });
        var noteClient = context.Client.GetProjectIssueNoteClient(project.Id);

        // Create
        var noteCreate = new ProjectIssueNoteCreate
        {
            IssueId = issue.IssueId,
            Body = "Issue Body",
        };

        var createdNote = noteClient.Create(noteCreate);
        Assert.That(createdNote.Body, Is.EqualTo(noteCreate.Body));

        // Get
        var getNote = noteClient.Get(issue.IssueId, createdNote.NoteId);
        Assert.That(getNote.Body, Is.EqualTo(noteCreate.Body));

        // Edit
        var noteEdit = new ProjectIssueNoteEdit
        {
            IssueId = issue.IssueId,
            NoteId = createdNote.NoteId,
            Body = "Modified Issue Body",
        };
        var editedNote = noteClient.Edit(noteEdit);
        Assert.That(editedNote.Body, Is.EqualTo(noteEdit.Body));
    }

    [Test]
    [NGitLabRetry]
    public async Task ListNotes()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var issue = context.Client.Issues.Create(new IssueCreate { ProjectId = project.Id, Title = "test", Description = "test" });
        var noteClient = context.Client.GetProjectIssueNoteClient(project.Id);

        var noteCreate = new ProjectIssueNoteCreate
        {
            IssueId = issue.IssueId,
            Body = "Issue Body",
        };

        noteClient.Create(noteCreate);
        noteClient.Create(noteCreate);

        var listedNote = noteClient.ForIssue(issue.IssueId).ToList();

        // GitLab create additional issue notes about initial manipulation on the issue,
        // so a starting count greater than one is expected.  Those issue notes have the 'system' boolean set to true.
        // We validate that there is only two issues that don't have the 'system' flag.
        Assert.That(listedNote.Where(x => !x.System).Count, Is.EqualTo(2));
        Assert.That(listedNote[0].Body, Is.EqualTo(noteCreate.Body));
    }
}
