using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class ProjectIssueNoteClient : ClientBase, IProjectIssueNoteClient
{
    private readonly long _projectId;

    public ProjectIssueNoteClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public Models.ProjectIssueNote Create(ProjectIssueNoteCreate create)
    {
        using (Context.BeginOperationScope())
        {
            var issue = GetIssue(create.IssueId);

            var projectIssueNote = new ProjectIssueNote
            {
                Body = create.Body,
                Confidential = create.Confidential,
                Author = Context.User,
            };
            issue.Notes.Add(projectIssueNote);

            return projectIssueNote.ToProjectIssueNote();
        }
    }

    public Models.ProjectIssueNote Edit(ProjectIssueNoteEdit edit)
    {
        using (Context.BeginOperationScope())
        {
            var note = GetIssueNote(edit.IssueId, edit.NoteId);

            note.Body = edit.Body;

            return note.ToProjectIssueNote();
        }
    }

    public IEnumerable<Models.ProjectIssueNote> ForIssue(long issueIid)
    {
        using (Context.BeginOperationScope())
        {
            return GetIssue(issueIid).Notes.Select(n => n.ToProjectIssueNote());
        }
    }

    public Models.ProjectIssueNote Get(long issueIid, long noteId)
    {
        using (Context.BeginOperationScope())
        {
            return GetIssueNote(issueIid, noteId).ToProjectIssueNote();
        }
    }

    private Issue GetIssue(long issueIid)
    {
        var project = GetProject(_projectId, ProjectPermission.View);
        var issue = project.Issues.FirstOrDefault(iss => iss.Iid == issueIid);

        if (issue == null)
        {
            throw new GitLabNotFoundException("Issue does not exist.");
        }

        return issue;
    }

    private ProjectIssueNote GetIssueNote(long issueIid, long issueNoteId)
    {
        var issue = GetIssue(issueIid);
        var note = issue.Notes.FirstOrDefault(n => n.Id == issueNoteId);

        if (note == null)
        {
            throw new GitLabNotFoundException("Issue Note does not exist.");
        }

        return note;
    }
}
