using System.Collections.Generic;
using System.Linq;

using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class ProjectIssueNoteClient : ClientBase, IProjectIssueNoteClient
    {
        private readonly int _projectId;

        public ProjectIssueNoteClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
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

        public IEnumerable<Models.ProjectIssueNote> ForIssue(int issueIid)
        {
            using (Context.BeginOperationScope())
            {
                return GetIssue(issueIid).Notes.Select(n => n.ToProjectIssueNote());
            }
        }

        public Models.ProjectIssueNote Get(int issueIid, int noteId)
        {
            using (Context.BeginOperationScope())
            {
                return GetIssueNote(issueIid, noteId).ToProjectIssueNote();
            }
        }

        private Issue GetIssue(int issueIid)
        {
            var project = GetProject(_projectId, ProjectPermission.View);
            var issue = project.Issues.FirstOrDefault(iss => iss.Iid == issueIid);

            if (issue == null)
            {
                throw new GitLabNotFoundException("Issue does not exist.");
            }

            return issue;
        }

        private ProjectIssueNote GetIssueNote(int issueIid, int issueNoteId)
        {
            var issue = GetIssue(issueIid);
            var note = issue.Notes.FirstOrDefault(n => n.Id == (long)issueNoteId);

            if (note == null)
            {
                throw new GitLabNotFoundException("Issue Note does not exist.");
            }

            return note;
        }
    }
}
