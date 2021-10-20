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

        public IEnumerable<Models.ProjectIssueNote> ForIssue(int issueId)
        {
            using (Context.BeginOperationScope())
            {
                return GetIssue(issueId).Notes.Select(n => n.ToProjectIssueNote());
            }
        }

        public Models.ProjectIssueNote Get(int issueId, int noteId)
        {
            using (Context.BeginOperationScope())
            {
                return GetIssueNote(issueId, noteId).ToProjectIssueNote();
            }
        }

        private Issue GetIssue(int issueId)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var issue = project.Issues.FirstOrDefault(iss => iss.Id == issueId);

            if (issue == null)
            {
                throw new GitLabNotFoundException("Issue does not exist.");
            }

            return issue;
        }

        private ProjectIssueNote GetIssueNote(int issueId, int issueNoteId)
        {
            var issue = GetIssue(issueId);
            var note = issue.Notes.FirstOrDefault(n => (int)n.Id == issueNoteId);

            if (note == null)
            {
                throw new GitLabNotFoundException("Issue Note does not exist.");
            }

            return note;
        }
    }
}
