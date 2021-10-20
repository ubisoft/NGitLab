using System;
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
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var issue = project.Issues.First(iss => iss.Id == create.IssueId);

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
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var issue = project.Issues.First(iss => iss.Id == edit.IssueId);
                var note = issue.Notes.First(n => (int)n.Id == edit.NoteId);

                note.Body = edit.Body;

                return note.ToProjectIssueNote();
            }
        }

        public IEnumerable<Models.ProjectIssueNote> ForIssue(int issueId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var issue = project.Issues.First(iss => iss.Id == issueId);

                return issue.Notes.Select(n => n.ToProjectIssueNote());
            }
        }

        public Models.ProjectIssueNote Get(int issueId, int noteId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Contribute);
                var issue = project.Issues.First(iss => iss.Id == issueId);

                return issue.Notes.First(n => (int)n.Id == noteId).ToProjectIssueNote();
            }
        }
    }
}
