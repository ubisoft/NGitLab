using System;
using System.Collections.Generic;
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

        public ProjectIssueNote Create(ProjectIssueNoteCreate create)
        {
            throw new NotImplementedException();
        }

        public ProjectIssueNote Edit(ProjectIssueNoteEdit edit)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProjectIssueNote> ForIssue(int issueId)
        {
            throw new NotImplementedException();
        }

        public ProjectIssueNote Get(int issueId, int noteId)
        {
            throw new NotImplementedException();
        }
    }
}
